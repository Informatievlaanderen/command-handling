namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EventHandling;
    using Framework;
    using global::SqlStreamStore;
    using NUnit.Framework;
    using Snapshotting;
    using StreamStoreStore.Json;

    [TestFixture]
    public class Construction
    {
        private ConcurrentUnitOfWork _unitOfWork;
        private Func<AggregateRootEntityStub> _factory;
        private IStreamStore _store;
        private EventDeserializer _eventDeserializer;
        private EventMapping _eventMapping;

        [SetUp]
        public void SetUp()
        {
            _store = new InMemoryStreamStore(() => DateTime.UtcNow);
            _unitOfWork = new ConcurrentUnitOfWork();
            _factory = () => new AggregateRootEntityStub();
            _eventDeserializer = new EventDeserializer(SimpleJson.DeserializeObject);
            _eventMapping = new EventMapping(new Dictionary<string, Type>());
        }

        [Test]
        public void FactoryCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Repository<AggregateRootEntityStub>(null, _unitOfWork, _store, _eventMapping, _eventDeserializer));
        }

        [Test]
        public void UnitOfWorkCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Repository<AggregateRootEntityStub>(_factory, null, _store, _eventMapping, _eventDeserializer));
        }

        [Test]
        public void EventStoreCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Repository<AggregateRootEntityStub>(_factory, _unitOfWork, null, _eventMapping, _eventDeserializer));
        }

        [Test]
        public void EventMappingCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Repository<AggregateRootEntityStub>(_factory, _unitOfWork, _store, null, _eventDeserializer));
        }

        [Test]
        public void EventDeserializerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Repository<AggregateRootEntityStub>(_factory, _unitOfWork, _store, _eventMapping, null));
        }

        [Test]
        public void UsingCtorReturnsInstanceWithExpectedProperties()
        {
            var sut = new Repository<AggregateRootEntityStub>(_factory, _unitOfWork, _store, _eventMapping, _eventDeserializer);
            Assert.That(sut.RootFactory, Is.SameAs(_factory));
            Assert.That(sut.UnitOfWork, Is.SameAs(_unitOfWork));
            Assert.That(sut.EventStore, Is.SameAs(_store));
            Assert.That(sut.EventMapping, Is.SameAs(_eventMapping));
            Assert.That(sut.EventDeserializer, Is.SameAs(_eventDeserializer));
        }
    }

    [TestFixture]
    public class WithEmptyStoreAndEmptyUnitOfWork
    {
        private Repository<AggregateRootEntityStub> _sut;
        private Model _model;

        [SetUp]
        public void SetUp()
        {
            var eventDeserializer = new EventDeserializer(SimpleJson.DeserializeObject);
            var eventMapping = new EventMapping(new Dictionary<string, Type>());
            _model = new Model();
            _sut = new RepositoryScenarioBuilder(eventMapping, eventDeserializer).BuildForRepository();
        }

        [Test]
        public void GetThrows()
        {
            var exception = Assert.ThrowsAsync<AggregateNotFoundException>(() => _sut.GetAsync(_model.UnknownIdentifier));
            Assert.That(exception.Identifier, Is.EqualTo(_model.UnknownIdentifier));
            Assert.That(exception.ClrType, Is.EqualTo(typeof(AggregateRootEntityStub)));
        }

        [Test]
        public async Task GetOptionalReturnsEmpty()
        {
            var result = await _sut.GetOptionalAsync(_model.UnknownIdentifier);

            Assert.That(result, Is.EqualTo(Optional<AggregateRootEntityStub>.Empty));
        }

        [Test]
        public void AddAttachesToUnitOfWork()
        {
            var root = new AggregateRootEntityStub();

            _sut.Add(_model.KnownIdentifier, root);

            var result = _sut.UnitOfWork.TryGet(_model.KnownIdentifier, out var aggregate);
            Assert.That(result, Is.True);
            Assert.That(aggregate.Identifier, Is.EqualTo(_model.KnownIdentifier));
            Assert.That(aggregate.Root, Is.SameAs(root));
        }
    }

    [TestFixture]
    public class WithEmptyStoreAndFilledUnitOfWork
    {
        private Repository<AggregateRootEntityStub> _sut;
        private AggregateRootEntityStub _root;
        private Model _model;

        [SetUp]
        public void SetUp()
        {
            var eventDeserializer = new EventDeserializer(SimpleJson.DeserializeObject);
            var eventMapping = new EventMapping(new Dictionary<string, Type>());

            _model = new Model();
            _root = new AggregateRootEntityStub();
            _sut = new RepositoryScenarioBuilder(eventMapping, eventDeserializer).
                ScheduleAttachToUnitOfWork(new Aggregate(_model.KnownIdentifier, 0, _root)).
                BuildForRepository();
        }

        [Test]
        public void GetThrowsForUnknownId()
        {
            var exception = Assert.ThrowsAsync<AggregateNotFoundException>(async () => await _sut.GetAsync(_model.UnknownIdentifier));
            Assert.That(exception.Identifier, Is.EqualTo(_model.UnknownIdentifier));
            Assert.That(exception.ClrType, Is.EqualTo(typeof(AggregateRootEntityStub)));
        }

        [Test]
        public async Task GetReturnsRootOfKnownId()
        {
            var result = await _sut.GetAsync(_model.KnownIdentifier);

            Assert.That(result, Is.SameAs(_root));
        }

        [Test]
        public async Task GetOptionalReturnsEmptyForUnknownId()
        {
            var result = await _sut.GetOptionalAsync(_model.UnknownIdentifier);

            Assert.That(result, Is.EqualTo(Optional<AggregateRootEntityStub>.Empty));
        }

        [Test]
        public async Task GetOptionalReturnsRootForKnownId()
        {
            var result = await _sut.GetOptionalAsync(_model.KnownIdentifier);

            Assert.That(result, Is.EqualTo(new Optional<AggregateRootEntityStub>(_root)));
        }
    }

    [TestFixture]
    public class WithStreamPresentInStore
    {
        private Repository<AggregateRootEntityStub> _sut;
        private Model _model;

        [SetUp]
        public void SetUp()
        {
            _model = new Model();

            var eventDeserializer = new EventDeserializer(SimpleJson.DeserializeObject);
            var eventMapping = new EventMapping(new Dictionary<string, Type>
            {
                { typeof(EventStub).AssemblyQualifiedName, typeof(EventStub) },
            });

            _sut = new RepositoryScenarioBuilder(eventMapping, eventDeserializer).
                ScheduleAppendToStream(_model.KnownIdentifier, new EventStub(1)).
                BuildForRepository();
        }

        [Test]
        public void GetThrowsForUnknownId()
        {
            var exception = Assert.ThrowsAsync<AggregateNotFoundException>(async () => await _sut.GetAsync(_model.UnknownIdentifier));
            Assert.That(exception.Identifier, Is.EqualTo(_model.UnknownIdentifier));
            Assert.That(exception.ClrType, Is.EqualTo(typeof(AggregateRootEntityStub)));
        }

        [Test]
        public async Task GetReturnsRootOfKnownId()
        {
            var result = await _sut.GetAsync(_model.KnownIdentifier);

            Assert.That(result.RecordedEvents, Is.EquivalentTo(new[] { new EventStub(1) }));
        }

        [Test]
        public async Task GetOptionalReturnsEmptyForUnknownId()
        {
            var result = await _sut.GetOptionalAsync(_model.UnknownIdentifier);

            Assert.That(result, Is.EqualTo(Optional<AggregateRootEntityStub>.Empty));
        }

        [Test]
        public async Task GetOptionalReturnsRootForKnownId()
        {
            var result = await _sut.GetOptionalAsync(_model.KnownIdentifier);

            Assert.That(result.HasValue, Is.True);
            Assert.That(result.Value.RecordedEvents, Is.EquivalentTo(new[] { new EventStub(1) }));
        }
    }

    [TestFixture]
    public class WithDeletedStreamInStore
    {
        private Repository<AggregateRootEntityStub> _sut;
        private Model _model;

        [SetUp]
        public void SetUp()
        {
            var eventDeserializer = new EventDeserializer(SimpleJson.DeserializeObject);
            var eventMapping = new EventMapping(new Dictionary<string, Type>());

            _model = new Model();
            _sut = new RepositoryScenarioBuilder(eventMapping, eventDeserializer).
                ScheduleAppendToStream(_model.KnownIdentifier, new EventStub(1)).
                ScheduleDeleteStream(_model.KnownIdentifier).
                BuildForRepository();
        }

        [Test]
        public void GetThrowsForUnknownId()
        {
            var exception =
                Assert.ThrowsAsync<AggregateNotFoundException>(async () => await _sut.GetAsync(_model.UnknownIdentifier));
            Assert.That(exception.Identifier, Is.EqualTo(_model.UnknownIdentifier));
            Assert.That(exception.ClrType, Is.EqualTo(typeof(AggregateRootEntityStub)));
        }

        [Test]
        public void GetThrowsForKnownDeletedId()
        {
            var exception = Assert.ThrowsAsync<AggregateNotFoundException>(() => _sut.GetAsync(_model.KnownIdentifier));
            Assert.That(exception.Identifier, Is.EqualTo(_model.KnownIdentifier));
            Assert.That(exception.ClrType, Is.EqualTo(typeof(AggregateRootEntityStub)));
        }

        [Test]
        public async Task GetOptionalReturnsEmptyForUnknownId()
        {
            var result = await _sut.GetOptionalAsync(_model.UnknownIdentifier);

            Assert.That(result, Is.EqualTo(Optional<AggregateRootEntityStub>.Empty));
        }

        [Test]
        public async Task GetOptionalReturnsEmptyForKnownDeletedId()
        {
            var result = await _sut.GetOptionalAsync(_model.KnownIdentifier);

            Assert.That(result, Is.EqualTo(Optional<AggregateRootEntityStub>.Empty));
        }
    }

    [TestFixture]
    public class WithSnapshotPresentInStore
    {
        private Repository<AggregateRootEntitySnapshotableStub> _sut;
        private Model _model;

        private readonly IEnumerable _events = new object[]
        {
            new SnapshotStub(1),
            new EventStub(5),
            new EventStub(6),
        };

        [SetUp]
        public void SetUp()
        {
            _model = new Model();

            var eventDeserializer = new EventDeserializer(SimpleJson.DeserializeObject);
            var eventSerializer = new EventSerializer(SimpleJson.SerializeObject);
            var eventMapping = new EventMapping(new Dictionary<string, Type>
            {
                { typeof(SnapshotStub).AssemblyQualifiedName, typeof(SnapshotStub) },
                { typeof(EventStub).AssemblyQualifiedName, typeof(EventStub) },
            });

            var snapshotContainer = new SnapshotContainer
            {
                Info =
                {
                    StreamVersion = 4,
                    Type = typeof(SnapshotStub).AssemblyQualifiedName
                },
                Data = eventSerializer.SerializeObject(new SnapshotStub(1))
            };

            _sut = new RepositoryScenarioBuilder(eventMapping, eventDeserializer)
                .ScheduleAppendToStream(_model.UnknownIdentifier, new EventStub(0))
                .ScheduleAppendToStream(_model.UnknownIdentifier, new EventStub(1))
                .ScheduleAppendToStream(_model.KnownIdentifier, new EventStub(0))
                .ScheduleAppendToStream(_model.KnownIdentifier, new EventStub(1))
                .ScheduleAppendToStream(_model.KnownIdentifier, new EventStub(2))
                .ScheduleAppendToStream(_model.KnownIdentifier, new EventStub(3))
                .ScheduleAppendToStream(_model.KnownIdentifier, new EventStub(4))
                .ScheduleAppendToStream($"{_model.KnownIdentifier}-snapshots", snapshotContainer)
                .ScheduleAppendToStream(_model.KnownIdentifier, new EventStub(5))
                .ScheduleAppendToStream(_model.KnownIdentifier, new EventStub(6))
                .BuildForRepository<AggregateRootEntitySnapshotableStub>(true);
        }

        [Test]
        public async Task GetReturnsRootOfKnownId()
        {
            var result = await _sut.GetAsync(_model.KnownIdentifier);

            Assert.That(result.RecordedEvents, Is.EquivalentTo(_events));
        }

        [Test]
        public async Task GetOptionalReturnsRootForKnownId()
        {
            var result = await _sut.GetOptionalAsync(_model.KnownIdentifier);

            Assert.That(result.HasValue, Is.True);
            Assert.That(result.Value.RecordedEvents, Is.EquivalentTo(_events));
        }
    }
}
