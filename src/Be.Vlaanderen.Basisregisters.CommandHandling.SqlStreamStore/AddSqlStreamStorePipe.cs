namespace Be.Vlaanderen.Basisregisters.CommandHandling.SqlStreamStore
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AggregateSource;
    using AggregateSource.Snapshotting;
    using EventHandling;
    using Generators.Guid;
    using global::SqlStreamStore;
    using global::SqlStreamStore.Streams;

    public static class AddSqlStreamStorePipe
    {
        public static ICommandHandlerBuilder<CommandMessage<TCommand>> AddSqlStreamStore<TCommand>(
            this ICommandHandlerBuilder<CommandMessage<TCommand>> commandHandlerBuilder,
            Func<IStreamStore> getStreamStore,
            Func<ConcurrentUnitOfWork> getUnitOfWork,
            EventMapping eventMapping,
            EventSerializer eventSerializer,
            Func<ISnapshotStore>? getSnapshotStore = null)
        {
            return commandHandlerBuilder.Pipe(next => async (commandMessage, ct) =>
            {
                await next(commandMessage, ct);

                return await AddSqlStreamStore(
                    getStreamStore,
                    getSnapshotStore,
                    getUnitOfWork,
                    eventMapping,
                    eventSerializer,
                    commandMessage,
                    ct);
            });
        }

        private static async Task<long> AddSqlStreamStore(
            Func<IStreamStore> getStreamStore,
            Func<ISnapshotStore>? getSnapshotStore,
            Func<ConcurrentUnitOfWork> getUnitOfWork,
            EventMapping eventMapping,
            EventSerializer eventSerializer,
            CommandMessage message,
            CancellationToken ct)
        {
            var uow = getUnitOfWork();

            var aggregate = uow.GetChanges().SingleOrDefault();
            if (aggregate == null)
            {
                return -1L;
            }

            var streamStore = getStreamStore();

            if (!message.Metadata.ContainsKey("CommandId"))
            {
                message.Metadata.Add("CommandId", message.CommandId);
            }

            var i = 1;

            var events = aggregate.Root.GetChangesWithMetadata().ToImmutableList();
            var changes = events
                .Select(o =>
                    new NewStreamMessage(
                        messageId: Deterministic.Create(Deterministic.Namespaces.Events, $"{message.CommandId}-{i++}"),
                        type: eventMapping.GetEventName(o.Event.GetType()),
                        jsonData: eventSerializer.SerializeObject(o.Event),
                        jsonMetadata: eventSerializer.SerializeObject(GetMetadata(message.Metadata, o.Metadata))))
                .ToArray();

            var result = await streamStore.AppendToStream(
                aggregate.Identifier,
                aggregate.ExpectedVersion,
                changes,
                ct);

            if (aggregate.Root is ISnapshotable support)
            {
                await CreateSnapshot(
                    support,
                    new SnapshotStrategyContext(
                        aggregate,
                        events,
                        result.CurrentVersion),
                    streamStore,
                    getSnapshotStore?.Invoke(),
                    uow,
                    eventMapping,
                    eventSerializer,
                    ct);
            }

            return result.CurrentPosition; // Position of the last event written
        }

        private static async Task CreateSnapshot(
            ISnapshotable snapshotSupport,
            SnapshotStrategyContext context,
            IStreamStore streamStore,
            ISnapshotStore? snapshotStore,
            ConcurrentUnitOfWork uow,
            EventMapping eventMapping,
            EventSerializer eventSerializer,
            CancellationToken ct)
        {
            if (!snapshotSupport.Strategy.ShouldCreateSnapshot(context))
            {
                return;
            }

            var snapshot = snapshotSupport.TakeSnapshot();
            if (snapshot == null)
            {
                throw new InvalidOperationException("Snapshot missing.");
            }

            var snapshotContainer = new SnapshotContainer
            {
                Data = eventSerializer.SerializeObject(snapshot),
                Info =
                {
                    Type = eventMapping.GetEventName(snapshot.GetType()),
                    StreamVersion = context.StreamVersion
                }
            };

            if (snapshotStore is not null)
            {
                await snapshotStore.SaveSnapshotAsync(context.Aggregate.Identifier, snapshotContainer, ct);
            }
            else
            {
                await streamStore.AppendToStream(
                    uow.GetSnapshotIdentifier(context.Aggregate.Identifier),
                    ExpectedVersion.Any,
                    new NewStreamMessage(
                        Deterministic.Create(Deterministic.Namespaces.Events, $"snapshot-{context.Aggregate.Identifier}-{context.StreamVersion}"),
                        $"SnapshotContainer<{snapshotContainer.Info.Type}>",
                        eventSerializer.SerializeObject(snapshotContainer)),
                    ct);
            }
        }

        private static IDictionary<string, object> GetMetadata(
            IDictionary<string, object> commandMetadata,
            IDictionary<string, object> eventMetadata)
        {
            // Merge metadata, event metadata takes presedence over commandmetadata
            return eventMetadata == null
                ? commandMetadata
                : eventMetadata.Union(commandMetadata, new KeyValuePairComparer<string, object>()).ToDictionary(x => x.Key, x => x.Value);
        }

        private class KeyValuePairComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
        {
            public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => x.Key.Equals(y.Key);

            public int GetHashCode(KeyValuePair<TKey, TValue> x) => x.GetHashCode();
        }
    }
}
