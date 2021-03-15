namespace Be.Vlaanderen.Basisregisters.CommandHandling.SqlStreamStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AggregateSource;
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
            EventSerializer eventSerializer)
        {
            return commandHandlerBuilder.Pipe(next => async (commandMessage, ct) =>
            {
                await next(commandMessage, ct);

                return await AddSqlStreamStore(
                    getStreamStore,
                    getUnitOfWork,
                    eventMapping,
                    eventSerializer,
                    commandMessage,
                    ct);
            });
        }

        private static async Task<long> AddSqlStreamStore(
            Func<IStreamStore> getStreamStore,
            Func<ConcurrentUnitOfWork> getUnitOfWork,
            EventMapping eventMapping,
            EventSerializer eventSerializer,
            CommandMessage message,
            CancellationToken ct)
        {
            var uow = getUnitOfWork();

            var aggregate = uow.GetChanges().SingleOrDefault();
            if (aggregate == null)
                return -1L;

            var streamStore = getStreamStore();

            if (!message.Metadata.ContainsKey("CommandId"))
                message.Metadata.Add("CommandId", message.CommandId);

            var i = 1;

            var changes = aggregate.Root.GetChangesWithMetadata()
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

            if (!aggregate.Root.EnableSnapshots)
                return result.CurrentPosition; // Position of the last event written

            var shouldCreateSnapshot = ShouldCreateSnapshot(
                result.CurrentPosition - changes.Length,
                result.CurrentPosition,
                aggregate.Root.SnapshotInterval);

            if (shouldCreateSnapshot)
                await CreateSnapshot(
                    streamStore,
                    uow,
                    eventMapping,
                    eventSerializer,
                    aggregate,
                    result.CurrentPosition,
                    ct);

            return result.CurrentPosition;
        }

        private static bool ShouldCreateSnapshot(
            long startPosition,
            long endPosition,
            int snapshotInterval)
        {
            for (var i = startPosition; i < endPosition; i++)
            {
                if (i % snapshotInterval == 0)
                    return true;
            }

            return false;
        }

        private static async Task CreateSnapshot(
            IStreamStore streamStore,
            ConcurrentUnitOfWork uow,
            EventMapping eventMapping,
            EventSerializer eventSerializer,
            Aggregate aggregate,
            long snapshotPosition,
            CancellationToken ct)
        {
            var snapshot = aggregate.Root.CreateSnapshot();
            if (snapshot == null)
                throw new InvalidOperationException("Snapshot missing.");

            var snapshotContainer = new SnapshotContainer
            {
                Data = eventSerializer.SerializeObject(snapshot),
                Info =
                {
                    Type = eventMapping.GetEventName(snapshot.GetType()),
                    Position = snapshotPosition
                }
            };

            await streamStore.AppendToStream(
                uow.GetSnapshotIdentifier(aggregate.Identifier),
                ExpectedVersion.Any,
                new NewStreamMessage(
                    Deterministic.Create(Deterministic.Namespaces.Events, $"snapshot-{snapshotPosition}"),
                    typeof(SnapshotContainer).AssemblyQualifiedName,
                    eventSerializer.SerializeObject(snapshotContainer)),
                ct);
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
