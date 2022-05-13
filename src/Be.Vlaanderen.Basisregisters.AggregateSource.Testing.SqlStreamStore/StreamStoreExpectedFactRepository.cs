namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EventHandling;
    using global::SqlStreamStore;
    using global::SqlStreamStore.Streams;

    public class StreamStoreExpectedFactRepository : IExpectedFactWriter, IExpectedFactReader
    {
        private readonly IStreamStore _streamStore;
        private readonly EventMapping _eventMapping;
        private readonly EventSerializer _eventSerializer;
        private readonly EventDeserializer _eventDeserializer;

        public StreamStoreExpectedFactRepository(
            IStreamStore streamStore,
            EventMapping eventMapping,
            EventSerializer eventSerializer,
            EventDeserializer eventDeserializer)
        {
            _streamStore = streamStore ?? throw new ArgumentNullException(nameof(streamStore));
            _eventMapping = eventMapping ?? throw new ArgumentNullException(nameof(eventMapping));
            _eventSerializer = eventSerializer ?? throw new ArgumentNullException(nameof(eventSerializer));
            _eventDeserializer = eventDeserializer ?? throw new ArgumentNullException(nameof(eventDeserializer));
        }

        public async Task<ExpectedFact[]> RetrieveFacts(long fromPositionExclusive)
        {
            var results = new List<ExpectedFact>();
            var page = await _streamStore.ReadAllForwards(fromPositionExclusive<0?Position.Start:fromPositionExclusive, 10);
            results.AddRange(page.Messages.Where(m => m.Position != fromPositionExclusive).Select(MapToFact));
            while (!page.IsEnd)
            {
                page = await page.ReadNext();
                results.AddRange(page.Messages.Where(m => m.Position != fromPositionExclusive).Select(MapToFact));
            }

            return results.ToArray();
        }

        public async Task<long> PersistFacts(ExpectedFact[] facts)
        {
            var factsByAggregate = facts.GroupBy(x => x.Identifier);

            AppendResult? result = null;
            foreach (var aggregateWithEvents in factsByAggregate)
            {
                result = await _streamStore.AppendToStream(
                    aggregateWithEvents.Key,
                    ExpectedVersion.Any,
                    aggregateWithEvents
                        .Select(e => new NewStreamMessage(
                            Guid.NewGuid(),
                            _eventMapping.GetEventName(e.Event.GetType()),
                            _eventSerializer.SerializeObject(e.Event))).ToArray());
            }

            return result?.CurrentPosition??await _streamStore.ReadHeadPosition();
        }

        private ExpectedFact MapToFact(StreamMessage streamMessage)
        {
            var eventType = _eventMapping.GetEventType(streamMessage.Type);
            var eventData = streamMessage.GetJsonData().GetAwaiter().GetResult();
            var @event = _eventDeserializer.DeserializeObject(eventData, eventType);

            return new ExpectedFact(streamMessage.StreamId, @event);
        }
    }
}
