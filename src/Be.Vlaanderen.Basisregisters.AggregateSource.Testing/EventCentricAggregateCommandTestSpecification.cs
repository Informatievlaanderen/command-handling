namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
    /// Represents an event centric test specification, meaning that the outcome revolves around events as the result of executing a command method on an aggregate.
    /// </summary>
    public class EventCentricAggregateCommandTestSpecification
    {
        /// <summary>
        /// Gets the sut factory.
        /// </summary>
        /// <value>
        /// The sut factory.
        /// </value>
        public Func<IAggregateRootEntity> SutFactory { get; }

        /// <summary>
        /// The events to arrange.
        /// </summary>
        public object[] Givens { get; }

        /// <summary>
        /// The command method to act upon.
        /// </summary>
        public Action<IAggregateRootEntity> When { get; }

        /// <summary>
        /// The expected events to assert.
        /// </summary>
        public object[] Thens { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCentricAggregateCommandTestSpecification"/> class.
        /// </summary>
        /// <param name="sutFactory">The sut factory.</param>
        /// <param name="givens">The events to arrange.</param>
        /// <param name="when">The command method to act upon.</param>
        /// <param name="thens">The events to assert.</param>
        public EventCentricAggregateCommandTestSpecification(
            Func<IAggregateRootEntity> sutFactory,
            object[] givens,
            Action<IAggregateRootEntity> when,
            object[] thens)
        {
            SutFactory = sutFactory ?? throw new ArgumentNullException(nameof(sutFactory));
            Givens = givens ?? throw new ArgumentNullException(nameof(givens));
            When = when ?? throw new ArgumentNullException(nameof(when));
            Thens = thens ?? throw new ArgumentNullException(nameof(thens));
        }

        /// <summary>
        /// Returns a test result that indicates this specification has passed.
        /// </summary>
        /// <returns>A new <see cref="EventCentricAggregateCommandTestResult"/>.</returns>
        public EventCentricAggregateCommandTestResult Pass()
            => new EventCentricAggregateCommandTestResult(
                this,
                TestResultState.Passed,
                Optional<object[]>.Empty,
                Optional<Exception>.Empty);

        /// <summary>
        /// Returns a test result that indicates this specification has failed because different things happened.
        /// </summary>
        /// <param name="actual">The actual events</param>
        /// <returns>A new <see cref="EventCentricAggregateCommandTestResult"/>.</returns>
        public EventCentricAggregateCommandTestResult Fail(object[] actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new EventCentricAggregateCommandTestResult(
                this,
                TestResultState.Failed,
                new Optional<object[]>(actual), 
                Optional<Exception>.Empty);
        }

        /// <summary>
        /// Returns a test result that indicates this specification has failed because an exception happened.
        /// </summary>
        /// <param name="actual">The actual exception</param>
        /// <returns>A new <see cref="EventCentricAggregateCommandTestResult"/>.</returns>
        public EventCentricAggregateCommandTestResult Fail(Exception actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new EventCentricAggregateCommandTestResult(
                this,
                TestResultState.Failed,
                Optional<object[]>.Empty,
                new Optional<Exception>(actual));
        }
    }
}
