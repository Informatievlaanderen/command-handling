namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHandling;
    using global::SqlStreamStore;
    using global::SqlStreamStore.Streams;
    using KellermanSoftware.CompareNetObjects;
    using Moq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using SqlStreamStore;
    using Testing.Comparers;

    [TestFixture]
    public class EventCentricTestSpecificationRunnerTests
    {
        private class FactComparer : IComparer<Fact>
        {
            public int Compare(Fact x, Fact y)
            {
                return new CompareNetObjectsBasedFactComparer(new CompareLogic()).Compare(x, y).Any() ? 1 : 0;
            }
        }

        [SetUp]
        public void CreateDependencies()
        {
            var store = new InMemoryStreamStore();
            var eventMapping = new EventMapping(new Dictionary<string, Type>
            {
                { "SomethingHappened", typeof(SomethingHappened) },
                { "SomethingElseHappened", typeof(SomethingElseHappened) },
            });
            var eventSerializer = new EventSerializer(JsonConvert.SerializeObject);
            var eventDeserializer = new EventDeserializer(JsonConvert.DeserializeObject);

            _factRepository = new StreamStoreFactRepository(store, eventMapping, eventSerializer, eventDeserializer);

            _handlerFactory = (eventType, @events) =>
                async (command) =>
                {
                    long position = 0;
                    foreach (var @event in events)
                    {
                        position = (await store.AppendToStream(
                                (command as DoSomething).Identifier,
                                ExpectedVersion.Any,
                                new NewStreamMessage(Guid.NewGuid(), eventType,
                                    eventSerializer.SerializeObject(@event)),
                                CancellationToken.None)
                            ).CurrentPosition;
                    }

                    return position;
                };
            _handlerResolver = new Mocking<IHandlerResolver, HandlerResolverSetup>();
            //_handlerResolver.When().ResolvesHandler(_handlerFactory);
        }

        private StreamStoreFactRepository _factRepository;
        private Func<string, object[], Func<object, Task<long>>> _handlerFactory;
        private Mocking<IHandlerResolver, HandlerResolverSetup> _handlerResolver;

        private EventCentricTestResult Run(EventCentricTestSpecification specification)
        {
            return new EventCentricTestSpecificationRunner(CreateFactComparer(), _factRepository, _factRepository,
                _handlerResolver.Object).Run(specification);
        }

        private Func<object, Task<long>> CreateHandlerThatRecords(string eventType, object @event)
        {
            return _handlerFactory(eventType, new []{@event});
        }

        private Func<object, Task<long>> CreateHandlerThatRecords(string eventType, object[] events)
        {
            return _handlerFactory(eventType, events);
        }

        protected virtual IFactComparer CreateFactComparer()
        {
            return new CompareNetObjectsBasedFactComparer(new CompareLogic());
        }

        [Test]
        public void FactComparerCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new EventCentricTestSpecificationRunner(null, _factRepository,
                _factRepository, _handlerResolver.Object));
        }

        [Test]
        public void FactWriterCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new EventCentricTestSpecificationRunner(CreateFactComparer(), null,
                _factRepository, _handlerResolver.Object));
        }

        [Test]
        public void FactReaderCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new EventCentricTestSpecificationRunner(CreateFactComparer(), _factRepository,
                null, _handlerResolver.Object));
        }

        [Test]
        public void HandlerResolverCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new EventCentricTestSpecificationRunner(CreateFactComparer(), _factRepository,
                _factRepository, null));
        }

        [Test]
        public void FailsIfExpectedEventsDoNotWhereRecorded()
        {
            _handlerResolver.When().ResolvesDummyHandler();

            var identifier = "1";
            var expectedEvents = new[] { new Fact(identifier, new SomethingHappened()) };

            var result = Run(new EventCentricTestSpecification(new Fact[0], new DoSomething { Identifier = identifier }, expectedEvents));

            ClassicAssert.IsTrue(result.Failed);
            ClassicAssert.IsTrue(result.ButEvents.HasValue);
        }

        [Test]
        public void FailsIfHandlerThrows()
        {
            var actualException = new Exception();
            _handlerResolver.When().HandlerThrows(actualException);

            var identifier = "1";
            var expectedEvents = new[] { new Fact(identifier, new SomethingHappened()) };

            var result = Run(new EventCentricTestSpecification(new Fact[0], new DoSomething() { Identifier = identifier }, expectedEvents));

            ClassicAssert.IsTrue(result.Failed);
            ClassicAssert.IsTrue(result.ButException.HasValue);
            ClassicAssert.AreEqual(actualException, result.ButException.Value);
        }

        [Test]
        public void FailsWithActualsIfOtherEventsWhereRecorded()
        {
            _handlerResolver.When()
                .ResolvesHandler(CreateHandlerThatRecords("SomethingElseHappened", new SomethingElseHappened()));

            var identifier = "1";
            var expectedEvents = new[] { new Fact(identifier, new SomethingHappened()) };

            var result = Run(new EventCentricTestSpecification(new Fact[0], new DoSomething() { Identifier = identifier }, expectedEvents));

            ClassicAssert.IsTrue(result.Failed);
            ClassicAssert.IsTrue(result.ButEvents.HasValue);
            Assert.That(result.ButEvents.Value, Is.EqualTo(new[] { new Fact(identifier, new SomethingElseHappened()) }).Using(new FactComparer()));
        }

        [Test]
        public void PassesIfExpectedEventsWhereRecorded()
        {
            _handlerResolver.When()
                .ResolvesHandler(CreateHandlerThatRecords("SomethingHappened", new SomethingHappened()));

            var identifier = "1";
            var expectedEvents = new[] { new Fact(identifier, new SomethingHappened()) };

            var result = Run(new EventCentricTestSpecification(new Fact[0], new DoSomething() { Identifier = identifier }, expectedEvents));

            ClassicAssert.IsTrue(result.Passed);
            ClassicAssert.IsTrue(result.ButEvents.HasValue);
            Assert.That(result.ButEvents.Value, Is.EqualTo(expectedEvents).Using(new FactComparer()));
        }

        [Test]
        public void PassesIfExpectedEventsWhereRecordedAndGivensNotEmtpy()
        {
            _handlerResolver.When()
                .ResolvesHandler(CreateHandlerThatRecords("SomethingHappened", new SomethingHappened()));

            var identifier = "1";
            var expectedEvents = new[] { new Fact(identifier, new SomethingHappened()) };

            var result = Run(new EventCentricTestSpecification(new Fact[]{new Fact(identifier, new SomethingHappened())}, new DoSomething() { Identifier = identifier }, expectedEvents));

            ClassicAssert.IsTrue(result.Passed);
            ClassicAssert.IsTrue(result.ButEvents.HasValue);
            Assert.That(result.ButEvents.Value, Is.EqualTo(expectedEvents).Using(new FactComparer()));
        }

        [Test]
        public void PassesIfExpectedEventsWhereRecordedAndStreamStoreNotEmpty()
        {
            var handler = CreateHandlerThatRecords("SomethingHappened", new SomethingHappened());
            _handlerResolver.When()
                .ResolvesHandler(handler);

            var identifier = "1";
            var expectedEvents = new[] { new Fact(identifier, new SomethingHappened()) };

            //put something in the event store
            handler(new DoSomething() { Identifier = identifier });

            var result = Run(new EventCentricTestSpecification(new Fact[0], new DoSomething() { Identifier = identifier }, expectedEvents));

            ClassicAssert.IsTrue(result.Passed);
            ClassicAssert.IsTrue(result.ButEvents.HasValue);
            Assert.That(result.ButEvents.Value, Is.EqualTo(expectedEvents).Using(new FactComparer()));
        }

        [Test]
        public void PassesIfExpectedEventsWhereRecordedAndGivensNotEmptyAndStreamStoreNotEmpty()
        {
            var handler = CreateHandlerThatRecords("SomethingHappened", new SomethingHappened());
            _handlerResolver.When()
                .ResolvesHandler(handler);

            var identifier = "1";
            var expectedEvents = new[] { new Fact(identifier, new SomethingHappened()) };

            //put something in the event store
            handler(new DoSomething() { Identifier = identifier });
            handler(new DoSomething() { Identifier = identifier });

            var result = Run(new EventCentricTestSpecification(new Fact[] { new Fact(identifier, new SomethingHappened()) }, new DoSomething() { Identifier = identifier }, expectedEvents));

            ClassicAssert.IsTrue(result.Passed);
            ClassicAssert.IsTrue(result.ButEvents.HasValue);
            Assert.That(result.ButEvents.Value, Is.EqualTo(expectedEvents).Using(new FactComparer()));
        }

        [Test]
        public void PassesIfManyRecordedEvents()
        {
            _handlerResolver.When()
                .ResolvesHandler(CreateHandlerThatRecords("SomethingHappened", Enumerable.Repeat(new SomethingHappened(), 17).ToArray()));

            var identifier = "1";
            var expectedEvents = Enumerable.Repeat(new Fact(identifier, new SomethingHappened()),17).ToArray();

            var result = Run(new EventCentricTestSpecification(new Fact[] { new Fact(identifier, new SomethingHappened()) }, new DoSomething() { Identifier = identifier }, expectedEvents));

            ClassicAssert.IsTrue(result.Passed);
            ClassicAssert.IsTrue(result.ButEvents.HasValue);
            Assert.That(result.ButEvents.Value, Is.EqualTo(expectedEvents).Using(new FactComparer()));
        }
    }
}
