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

    public class StreamStoreCommandHandlerModule<T> : CommandHandlerModule where T : CommandHandlerModule
    {
        private readonly EventMapping _eventMapping;
        private readonly EventSerializer _eventSerializer;

        public StreamStoreCommandHandlerModule(
            Func<IStreamStore> getStreamStore,
            Func<ConcurrentUnitOfWork> getUnitOfWork,
            Func<ReturnHandler<CommandMessage>, T> innerModuleFactory,
            EventMapping eventMapping,
            EventSerializer eventSerializer)
        {
            _eventMapping = eventMapping;
            _eventSerializer = eventSerializer;
            Wrap(innerModuleFactory((message, ct) => ToStreamStore(getStreamStore, getUnitOfWork, message, ct)));
        }

        private async Task<long> ToStreamStore(Func<IStreamStore> getStreamStore, Func<ConcurrentUnitOfWork> getUnitOfWork, CommandMessage message, CancellationToken ct)
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
                            messageId: Deterministic.Create(Deterministic.Namespaces.Events, $"{message.CommandId}-{i++}"),
                            type: _eventMapping.GetEventName(o.Event.GetType()),
                            jsonData: _eventSerializer.SerializeObject(o.Event),
                            jsonMetadata: _eventSerializer.SerializeObject(GetMetadata(message.Metadata, o.Metadata))))
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
