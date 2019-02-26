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
            var aggregate = getUnitOfWork().GetChanges().SingleOrDefault();
            if (aggregate == null)
                return -1L;

            if (!message.Metadata.ContainsKey("CommandId"))
                message.Metadata.Add("CommandId", message.CommandId);

            var i = 1;
            var result = await getStreamStore().AppendToStream(
                aggregate.Identifier,
                aggregate.ExpectedVersion,
                aggregate.Root.GetChangesWithMetadata()
                    .Select(o =>
                        new NewStreamMessage(
                            messageId: Deterministic.Create(Deterministic.Namespaces.Events,
                                $"{message.CommandId}-{i++}"),
                            type: eventMapping.GetEventName(o.Event.GetType()),
                            jsonData: eventSerializer.SerializeObject(o.Event),
                            jsonMetadata: eventSerializer.SerializeObject(GetMetadata(message.Metadata, o.Metadata))))
                    .ToArray(), ct);

            return result.CurrentPosition;
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
