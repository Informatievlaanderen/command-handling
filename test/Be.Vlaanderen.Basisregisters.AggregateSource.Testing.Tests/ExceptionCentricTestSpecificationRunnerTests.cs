namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    using System;
    using System.Collections.Generic;
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
    public class ExceptionCentricTestSpecificationRunnerTests
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

            _handlerFactory = (eventType, @event) =>
                 async (command) =>
                     (await store.AppendToStream(
                         (command as DoSomething).Identifier,
                         ExpectedVersion.Any,
                         new NewStreamMessage(Guid.NewGuid(), eventType, eventSerializer.SerializeObject(@event)), CancellationToken.None)
                     ).CurrentPosition;
            _handlerResolver = new Mocking<IHandlerResolver, HandlerResolverSetup>();
        }

        private StreamStoreFactRepository _factRepository;
        private Func<string, object, Func<object, Task<long>>> _handlerFactory;
        private Mocking<IHandlerResolver, HandlerResolverSetup> _handlerResolver;

        private Func<object, Task<long>> CreateHandlerThatRecords(string eventType, object @event)
        {
            return _handlerFactory(eventType, @event);
        }

        private ExceptionCentricTestResult Run(ExceptionCentricTestSpecification specification)
        {
            return new ExceptionCentricTestSpecificationRunner(CreateExceptionComparer(), _factRepository,
                _factRepository, _handlerResolver.Object).Run(specification);
        }

        protected virtual IExceptionComparer CreateExceptionComparer()
        {
            var comparer = new CompareLogic();
            comparer.Config.MembersToIgnore.Add("Source");
            comparer.Config.MembersToIgnore.Add("StackTrace");
            comparer.Config.MembersToIgnore.Add("TargetSite");
            return new CompareNetObjectsBasedExceptionComparer(comparer);
        }

        [Test]
        public void ExceptionComparerCannotBeNull()
        {
            ClassicAssert.Throws<ArgumentNullException>(() => new ExceptionCentricTestSpecificationRunner(null, _factRepository,
                _factRepository, _handlerResolver.Object));
        }

        [Test]
        public void FactWriterCannotBeNull()
        {
            ClassicAssert.Throws<ArgumentNullException>(() => new ExceptionCentricTestSpecificationRunner(CreateExceptionComparer(), null,
                _factRepository, _handlerResolver.Object));
        }

        [Test]
        public void FactReaderCannotBeNull()
        {
            ClassicAssert.Throws<ArgumentNullException>(() => new ExceptionCentricTestSpecificationRunner(CreateExceptionComparer(), _factRepository,
                null, _handlerResolver.Object));
        }

        [Test]
        public void HandlerResolverCannotBeNull()
        {
            ClassicAssert.Throws<ArgumentNullException>(() => new ExceptionCentricTestSpecificationRunner(CreateExceptionComparer(), _factRepository,
                _factRepository, null));
        }

        [Test]
        public void FailsIfNoExceptionIsThrown()
        {
            var expectedException = new Exception();
            _handlerResolver.When().ResolvesDummyHandler();

            var result = Run(new ExceptionCentricTestSpecification(new Fact[0], new DoSomething(){Identifier = "1"}, expectedException));

            ClassicAssert.IsTrue(result.Failed);
            ClassicAssert.IsFalse(result.ButException.HasValue);
            ClassicAssert.IsFalse(result.ButEvents.HasValue);
        }

        [Test]
        public void FailsWithActualEventsIfNoExceptionIsThrown()
        {
            var expectedException = new Exception();
            var identifier = "1";
            _handlerResolver.When().ResolvesHandler(CreateHandlerThatRecords("SomethingHappened", new SomethingHappened()));

            var result = Run(new ExceptionCentricTestSpecification(new Fact[0], new DoSomething(){Identifier = identifier}, expectedException));

            ClassicAssert.IsTrue(result.Failed);
            ClassicAssert.IsFalse(result.ButException.HasValue);
            ClassicAssert.IsTrue(result.ButEvents.HasValue);
            ClassicAssert.That(result.ButEvents.Value, Is.EqualTo(new[] { new Fact(identifier, new SomethingHappened()) }).Using(new FactComparer()));
        }

        [Test]
        public void FailsWithActualExceptionIfOtherExceptionIsThrown()
        {
            var expectedException = new Exception();
            var actualException = new Exception("not the expected exception");
            _handlerResolver.When().HandlerThrows(actualException);

            var result = Run(new ExceptionCentricTestSpecification(new Fact[0], new DoSomething(), expectedException));

            ClassicAssert.IsTrue(result.Failed);
            ClassicAssert.IsTrue(result.ButException.HasValue);
            ClassicAssert.AreEqual(actualException, result.ButException.Value);
        }

        [Test]
        public void PassesIfExpectedExceptionIsThrown()
        {
            var expectedException = new Exception();

            _handlerResolver.When().HandlerThrows(expectedException);

            var result = Run(new ExceptionCentricTestSpecification(new Fact[0], new DoSomething(), expectedException));

            ClassicAssert.IsTrue(result.Passed);
            ClassicAssert.IsTrue(result.ButException.HasValue);
        }
    }
}
