namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AggregateSource;
    using EventHandling;
    using SqlStreamStore;
    using SqlStreamStore.Streams;

    public class AggregateEventsRepository<TAggregateRoot, TStreamId> : IAggregateEventsRepository<TAggregateRoot, TStreamId>
        where TAggregateRoot : class, IAggregateRootEntity, ISnapshotable
        where TStreamId : class
    {
        private readonly EventDeserializer _eventDeserializer;
        private readonly EventMapping _eventMapping;
        private readonly IReadonlyStreamStore _streamStore;
        private readonly Func<TAggregateRoot> _aggregateRootFactory;

        public AggregateEventsRepository(
            EventDeserializer eventDeserializer,
            EventMapping eventMapping,
            IReadonlyStreamStore streamStore,
            Func<TAggregateRoot> aggregateRootFactory)
        {
            _eventDeserializer = eventDeserializer;
            _eventMapping = eventMapping;
            _streamStore = streamStore;

            _aggregateRootFactory = aggregateRootFactory;
        }

        public async Task<TAggregateRoot?> GetAggregateByEvents(
            TStreamId streamId,
            int snapshotAggregateStreamVersion,
            CancellationToken stoppingToken)
        {
            var page = await _streamStore.ReadStreamBackwards(streamId.ToString(), snapshotAggregateStreamVersion, 100,
                stoppingToken);
            if (page.Status == PageReadStatus.StreamNotFound)
            {
                return null;
            }

            var aggregate = _aggregateRootFactory.Invoke();
            var events = new List<object>();
            events.AddRange(await ParseEvents(page, stoppingToken));
            while (!page.IsEnd)
            {
                page = await page.ReadNext(stoppingToken);
                events.AddRange(await ParseEvents(page, stoppingToken));
            }

            events.Reverse(); // events are read backwards, so reverse them to get them in the correct order
            aggregate.Initialize(events);

            return aggregate;
        }

        private async Task<IEnumerable<object>> ParseEvents(
            ReadStreamPage page,
            CancellationToken cancellationToken)
        {
            var events = new List<object>();

            foreach (var message in page.Messages)
            {
                var eventType = _eventMapping.GetEventType(message.Type);
                var eventData = await message.GetJsonData(cancellationToken);
                events.Add(_eventDeserializer.DeserializeObject(eventData, eventType));
            }

            return events;
        }
    }
}
