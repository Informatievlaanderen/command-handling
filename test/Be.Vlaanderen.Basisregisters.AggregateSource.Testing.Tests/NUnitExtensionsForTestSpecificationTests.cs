namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    namespace NUnitExtensionsForTestSpecificationTests
    {
        using System;
        using System.Collections.Generic;
        using Microsoft.Extensions.Logging;
        using Moq;
        using NUnit.Framework;

        public class ConsoleLogger : ILogger
        {
            private readonly Action<string> _logger;

            public ConsoleLogger()
            {
                _logger = Console.WriteLine;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                _logger(formatter(state, exception));
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }
        }

        [TestFixture]
        public class EventCentricAssert
        {
            private Mocking<IEventCentricTestSpecificationRunner, EventCentricTestSpecificationRunnerSetup> _runner;

            private class EqualsFactComparer : IFactComparer
            {
                public IEnumerable<FactComparisonDifference> Compare(Fact expected, Fact actual)
                {
                    if (!expected.Equals(actual))
                        yield return new FactComparisonDifference(expected, actual, "-");
                }
            }

            private class NullBuilder : IEventCentricTestSpecificationBuilder
            {
                public static readonly IEventCentricTestSpecificationBuilder Instance =
                    new NullBuilder();

                public EventCentricTestSpecification Build()
                {
                    return null;
                }
            }

            private class EventCentricTestSpecificationRunnerSetup : MockingSetup<IEventCentricTestSpecificationRunner>
            {
                public EventCentricTestSpecificationRunnerSetup SpecificationRunFailsWithEvents(Fact[] events)
                {
                    Moq.Setup(x => x.Run(It.IsAny<EventCentricTestSpecification>()))
                        .Returns<EventCentricTestSpecification>(spec => spec.Fail(events));

                    return this;
                }

                public EventCentricTestSpecificationRunnerSetup SpecificationRunFailsWithException(Exception exception)
                {
                    Moq.Setup(x => x.Run(It.IsAny<EventCentricTestSpecification>()))
                        .Returns<EventCentricTestSpecification>(spec => spec.Fail(exception));

                    return this;
                }

                public EventCentricTestSpecificationRunnerSetup SpecificationRunPasses()
                {
                    Moq.Setup(x => x.Run(It.IsAny<EventCentricTestSpecification>()))
                        .Returns<EventCentricTestSpecification>(spec => spec.Pass());

                    return this;
                }

                public EventCentricTestSpecificationRunnerSetup SpecificationRunPassesWithEvents(Fact[] events)
                {
                    Moq.Setup(x => x.Run(It.IsAny<EventCentricTestSpecification>()))
                        .Returns<EventCentricTestSpecification>(spec => spec.Pass(events));

                    return this;
                }
            }

            [SetUp]
            public void CreateMocks()
            {
                _runner = new Mocking<IEventCentricTestSpecificationRunner, EventCentricTestSpecificationRunnerSetup>();
            }

            [Test]
            public void BuilderCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => ((IEventCentricTestSpecificationBuilder) null).Assert(_runner.Object, new EqualsFactComparer(), new ConsoleLogger()));
            }

            [Test]
            public void RunnnerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => NullBuilder.Instance.Assert(null, new EqualsFactComparer(), new ConsoleLogger()));
            }

            [Test]
            public void ComparerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => NullBuilder.Instance.Assert(_runner.Object, null, new ConsoleLogger()));
            }

            [Test]
            public void LoggerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => NullBuilder.Instance.Assert(_runner.Object, new EqualsFactComparer(), null));
            }

            [Test]
            public void WhenSpecificationRunFailsWithEventCountDifference()
            {
                _runner.When().SpecificationRunFailsWithEvents(new[]
                    {new Fact("1", new SomethingHappened()), new Fact("1", new SomethingElseHappened())});

                Assert.Throws<AssertionException>(
                    () =>
                        new Scenario().GivenNone().When(new DoSomething()).Then("1", new SomethingHappened())
                            .Assert(_runner.Object, new EqualsFactComparer(), new ConsoleLogger()));
            }

            [Test]
            public void WhenSpecificationRunFailsWithEventDifferences()
            {
                _runner.When().SpecificationRunFailsWithEvents(new[]
                    {new Fact("1", new SomethingElseHappened())});

                Assert.Throws<AssertionException>(
                    () =>
                        new Scenario().GivenNone().When(new DoSomething()).Then("1", new SomethingHappened())
                            .Assert(_runner.Object, new EqualsFactComparer(), new ConsoleLogger()));
            }

            [Test]
            public void WhenSpecificationRunFailsWithException()
            {
                _runner.When().SpecificationRunFailsWithException(new Exception());

                Assert.Throws<AssertionException>(
                    () =>
                        new Scenario().GivenNone().When(new DoSomething()).Then("1", new SomethingHappened())
                            .Assert(_runner.Object, new EqualsFactComparer(), new ConsoleLogger()));
            }

            [Test]
            public void WhenSpecificationRunPasses()
            {
                _runner.When().SpecificationRunPasses();

                Assert.DoesNotThrow(() =>
                    new Scenario().GivenNone().When(new DoSomething()).Then("1", new SomethingHappened())
                        .Assert(_runner.Object, new EqualsFactComparer(), new ConsoleLogger()));
            }

            [Test]
            public void WhenSpecificationRunPassesWithEvents()
            {
                _runner.When().SpecificationRunPassesWithEvents(new[]
                    {new Fact("1", new SomethingHappened())});

                Assert.DoesNotThrow(() =>
                    new Scenario().GivenNone().When(new DoSomething()).Then("1", new SomethingHappened())
                        .Assert(_runner.Object, new EqualsFactComparer(), new ConsoleLogger()));
            }

        }

        [TestFixture]
        public class ExceptionCentricAssert
        {
            private Mocking<IExceptionCentricTestSpecificationRunner, ExceptionCentricTestSpecificationRunnerSetup> _runner;

            private class EqualsExceptionComparer : IExceptionComparer
            {
                public IEnumerable<ExceptionComparisonDifference> Compare(Exception expected, Exception actual)
                {
                    if (!expected.Equals(actual))
                        yield return new ExceptionComparisonDifference(expected, actual, "-");
                }
            }

            private class NullBuilder : IExceptionCentricTestSpecificationBuilder
            {
                public static readonly IExceptionCentricTestSpecificationBuilder Instance =
                    new NullBuilder();

                public ExceptionCentricTestSpecification Build()
                {
                    return null;
                }
            }

            private class ExceptionCentricTestSpecificationRunnerSetup : MockingSetup<IExceptionCentricTestSpecificationRunner>
            {
                public ExceptionCentricTestSpecificationRunnerSetup SpecificationRunFailsBecauseException(Exception exception)
                {
                    Moq.Setup(x => x.Run(It.IsAny<ExceptionCentricTestSpecification>()))
                        .Returns<ExceptionCentricTestSpecification>(spec => spec.Fail(exception));

                    return this;
                }

                public ExceptionCentricTestSpecificationRunnerSetup SpecificationRunFailsBecauseEvents(Fact[] events)
                {
                    Moq.Setup(x => x.Run(It.IsAny<ExceptionCentricTestSpecification>()))
                        .Returns<ExceptionCentricTestSpecification>(spec => spec.Fail(events));

                    return this;
                }

                public ExceptionCentricTestSpecificationRunnerSetup SpecificationRunFails()
                {
                    Moq.Setup(x => x.Run(It.IsAny<ExceptionCentricTestSpecification>()))
                        .Returns<ExceptionCentricTestSpecification>(spec => spec.Fail());

                    return this;
                }

                public ExceptionCentricTestSpecificationRunnerSetup SpecificationRunPasses(Exception exception)
                {
                    Moq.Setup(x => x.Run(It.IsAny<ExceptionCentricTestSpecification>()))
                        .Returns<ExceptionCentricTestSpecification>(spec => spec.Pass(exception));

                    return this;
                }
            }

            [SetUp]
            public void CreateMocks()
            {
                _runner = new Mocking<IExceptionCentricTestSpecificationRunner, ExceptionCentricTestSpecificationRunnerSetup>();
            }

            [Test]
            public void BuilderCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => ((IExceptionCentricTestSpecificationBuilder)null).Assert(_runner.Object, new EqualsExceptionComparer(), new ConsoleLogger()));
            }

            [Test]
            public void RunnnerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => NullBuilder.Instance.Assert(null, new EqualsExceptionComparer(), new ConsoleLogger()));
            }

            [Test]
            public void ComparerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => NullBuilder.Instance.Assert(_runner.Object, null, new ConsoleLogger()));
            }

            [Test]
            public void LoggerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => NullBuilder.Instance.Assert(_runner.Object, new EqualsExceptionComparer(), null));
            }

            [Test]
            public void WhenSpecificationRunFailsBecauseDifferentException()
            {
                _runner.When()
                    .SpecificationRunFailsBecauseException(new Exception("this is not the expected exception"));
                Assert.Throws<AssertionException>(
                    () =>
                        new Scenario()
                            .GivenNone()
                            .When(new DoSomething())
                            .Throws(new Exception())
                            .Assert(_runner.Object, new EqualsExceptionComparer(), new ConsoleLogger()));
            }

            [Test]
            public void WhenSpecificationRunFailsBecauseEvents()
            {
                _runner.When()
                    .SpecificationRunFailsBecauseEvents(new []{new Fact("1", new SomethingHappened())});
                Assert.Throws<AssertionException>(
                    () =>
                        new Scenario()
                            .GivenNone()
                            .When(new DoSomething())
                            .Throws(new Exception())
                            .Assert(_runner.Object, new EqualsExceptionComparer(), new ConsoleLogger()));
            }

            [Test]
            public void WhenSpecificationRunFailsBecauseNoException()
            {
                _runner.When()
                    .SpecificationRunFails();
                Assert.Throws<AssertionException>(
                    () =>
                        new Scenario()
                            .GivenNone()
                            .When(new DoSomething())
                            .Throws(new Exception())
                            .Assert(_runner.Object, new EqualsExceptionComparer(), new ConsoleLogger()));
            }

            [Test]
            public void WhenSpecificationRunPasses()
            {
                var expectedException= new Exception();
                _runner.When().SpecificationRunPasses(expectedException);
                Assert.DoesNotThrow(
                    () =>
                        new Scenario()
                            .GivenNone()
                            .When(new DoSomething())
                            .Throws(expectedException)
                            .Assert(_runner.Object, new EqualsExceptionComparer(), new ConsoleLogger()));
            }
        }
    }
}
