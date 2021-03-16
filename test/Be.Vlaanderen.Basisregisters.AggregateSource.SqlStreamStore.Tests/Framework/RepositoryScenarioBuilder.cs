namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Tests.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using EventHandling;
    using StreamStoreStore.Json;
    using global::SqlStreamStore;
    using global::SqlStreamStore.Streams;

    public class RepositoryScenarioBuilder
    {
        private readonly IStreamStore _eventStore;
        private readonly List<Action<IStreamStore>> _eventStoreSchedule;
        private readonly List<Action<ConcurrentUnitOfWork>> _unitOfWorkSchedule;
        private ConcurrentUnitOfWork _unitOfWork;
        private readonly EventMapping _eventMapping;
        private readonly EventDeserializer _eventDeserializer;

        public RepositoryScenarioBuilder(EventMapping eventMapping, EventDeserializer eventDeserializer)
        {
            _eventStore = new InMemoryStreamStore(() => DateTime.UtcNow);
            _unitOfWork = new ConcurrentUnitOfWork();
            _eventStoreSchedule = new List<Action<IStreamStore>>();
            _unitOfWorkSchedule = new List<Action<ConcurrentUnitOfWork>>();
            _eventMapping = eventMapping;
            _eventDeserializer = eventDeserializer;
        }

        public RepositoryScenarioBuilder WithUnitOfWork(ConcurrentUnitOfWork value)
        {
            _unitOfWork = value;
            return this;
        }

        public RepositoryScenarioBuilder ScheduleAppendToStream(string stream, params object[] events)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (events == null)
                throw new ArgumentNullException(nameof(events));

            _eventStoreSchedule.Add(
                store =>
                {
                    var messages = events
                        .Select(o =>
                            new NewStreamMessage(
                                messageId: Guid.NewGuid(),
                                type: o.GetType().AssemblyQualifiedName,
                                jsonData: SimpleJson.SerializeObject(o)))
                        .ToList();

                    store.AppendToStream(new StreamId(stream), ExpectedVersion.Any, messages.ToArray(), CancellationToken.None).GetAwaiter().GetResult();
                });

            return this;
        }

        public RepositoryScenarioBuilder ScheduleDeleteStream(string stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            _eventStoreSchedule.Add(store => store.DeleteStream(stream).GetAwaiter().GetResult());
            return this;
        }

        public RepositoryScenarioBuilder ScheduleAttachToUnitOfWork(Aggregate aggregate)
        {
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            _unitOfWorkSchedule.Add(uow => uow.Attach(aggregate));
            return this;
        }

        public Repository<T> BuildForRepository<T>()
            where T : AggregateRootEntityStub, new ()
        {
            ExecuteScheduledActions();
            return new Repository<T>(
                () => new T(),
                _unitOfWork,
                _eventStore,
                _eventMapping,
                _eventDeserializer);
        }
        public Repository<AggregateRootEntityStub> BuildForRepository()
            => BuildForRepository<AggregateRootEntityStub>();

        private void ExecuteScheduledActions()
        {
            foreach (var action in _eventStoreSchedule)
                action(_eventStore);

            foreach (var action in _unitOfWorkSchedule)
                action(_unitOfWork);
        }
    }
}
