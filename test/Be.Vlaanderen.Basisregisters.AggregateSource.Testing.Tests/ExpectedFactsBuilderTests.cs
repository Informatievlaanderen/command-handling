namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    using System;
    using NUnit.Framework;

    namespace ExpectedFactsBuilderTests
    {
        [TestFixture]
        public class InitialInstanceTests
        {
            ExpectedFactsBuilder? _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new ExpectedFactsBuilder();
            }

            [Test]
            public void IsEmpty()
            {
                ExpectedFact[] result = _sut;

                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class StateThatEventsTests : ThatEventsFixture {
            public override ExpectedFactsBuilder ExpectedFact(string identifier, params object[] events)
            {
                return State.That(identifier, events);
            }
        }

        [TestFixture]
        public class ExpectedFactsBuilderThatEventsTests : ThatEventsFixture {
            public override ExpectedFactsBuilder ExpectedFact(string identifier, params object[] events)
            {
                return new ExpectedFactsBuilder().That(identifier, events);
            }
        }

        public abstract class ThatEventsFixture
        {
            public abstract ExpectedFactsBuilder ExpectedFact(string identifier, params object[] events);

            [Test]
            public void IdentifierCannotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => ExpectedFact(null, new object[0]));
            }

            [Test]
            public void EventsCannotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => ExpectedFact(Model.Identifier1, null));
            }

            [Test]
            public void WhenNoEventsAreSpecifiedThenReturnsEmpty()
            {
                ExpectedFact[] result = ExpectedFact(Model.Identifier1, Array.Empty<object>());

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenEventsAreSpecifiedThenReturnsExpectedResult()
            {
                var event1 = new object();
                var event2 = new object();

                ExpectedFact[] result = ExpectedFact(Model.Identifier1, event1, event2);

                Assert.That(result, Is.EquivalentTo(
                    new[]
                    {
                        new ExpectedFact(Model.Identifier1, event1),
                        new ExpectedFact(Model.Identifier1, event2)
                    }));
            }

            [Test]
            public void WhenNewEventsAreSpecifiedThenReturnsCombinedExpectedResult()
            {
                var event1 = new object();
                var event2 = new object();

                var sut = ExpectedFact(Model.Identifier1, event1, event2);

                var event3 = new object();
                var event4 = new object();

                ExpectedFact[] result = sut.That(Model.Identifier2, event3, event4);

                Assert.That(result, Is.EquivalentTo(
                    new[]
                    {
                        new ExpectedFact(Model.Identifier1, event1),
                        new ExpectedFact(Model.Identifier1, event2),
                        new ExpectedFact(Model.Identifier2, event3),
                        new ExpectedFact(Model.Identifier2, event4)
                    }));
            }
        }

        [TestFixture]
        public class StateThatFactsTests : ThatFactsFixture
        {
            public override ExpectedFactsBuilder ExpectedFacts(params ExpectedFact[] facts)
            {
                return State.That(facts);
            }
        }

        [TestFixture]
        public class FactsBuilderThatFactsTests : ThatFactsFixture
        {
            public override ExpectedFactsBuilder ExpectedFacts(params ExpectedFact[] facts)
            {
                return new ExpectedFactsBuilder().That(facts);
            }
        }

        public abstract class ThatFactsFixture
        {
            public abstract ExpectedFactsBuilder ExpectedFacts(params ExpectedFact[] facts);

            [Test]
            public void ExpectedFactsCannotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => ExpectedFacts(null));
            }

            [Test]
            public void WhenNoExpectedFactsAreSpecifiedThenReturnsEmpty()
            {
                ExpectedFact[] result = ExpectedFacts();

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenExpectedFactsAreSpecifiedThenReturnsExpectedResult()
            {
                var fact1 = new ExpectedFact(Model.Identifier1, new object());
                var fact2 = new ExpectedFact(Model.Identifier2, new object());

                ExpectedFact[] result = ExpectedFacts(fact1, fact2);

                Assert.That(result, Is.EquivalentTo(
                    new[]
                    {
                        fact1,
                        fact2
                    }));
            }

            [Test]
            public void WhenNewExpectedFactsAreSpecifiedThenReturnsCombinedExpectedResult()
            {
                var fact1 = new ExpectedFact(Model.Identifier1, new object());
                var fact2 = new ExpectedFact(Model.Identifier2, new object());

                var sut = ExpectedFacts(fact1, fact2);

                var fact3 = new ExpectedFact(Model.Identifier1, new object());
                var fact4 = new ExpectedFact(Model.Identifier2, new object());

                ExpectedFact[] result = sut.That(fact3, fact4);

                Assert.That(result, Is.EquivalentTo(new[] {fact1, fact2, fact3, fact4}));
            }
        }
    }
}
