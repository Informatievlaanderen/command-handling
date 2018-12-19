namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
    /// Represents an event centric test specification, meaning that the outcome revolves around events as the result of executing a factory method on an aggregate.
    /// </summary>
    public class EventCentricAggregateFactoryTestSpecification
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
        /// The factory method to act upon.
        /// </summary>
        public Func<IAggregateRootEntity, IAggregateRootEntity> When { get; }

        /// <summary>
        /// The expected events to assert.
        /// </summary>
        public object[] Thens { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCentricAggregateFactoryTestSpecification"/> class.
        /// </summary>
        /// <param name="sutFactory">The sut factory.</param>
        /// <param name="givens">The events to arrange.</param>
        /// <param name="when">The factory method to act upon.</param>
        /// <param name="thens">The events to assert.</param>
        public EventCentricAggregateFactoryTestSpecification(
            Func<IAggregateRootEntity> sutFactory,
            object[] givens,
            Func<IAggregateRootEntity, IAggregateRootEntity> when,
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
        /// <returns>A new <see cref="EventCentricAggregateFactoryTestResult"/>.</returns>
        public EventCentricAggregateFactoryTestResult Pass()
            => new EventCentricAggregateFactoryTestResult(
                this,
                TestResultState.Passed,
                Optional<object[]>.Empty,
                Optional<Exception>.Empty);

        /// <summary>
        /// Returns a test result that indicates this specification has failed because different things happened.
        /// </summary>
        /// <param name="actual">The actual events</param>
        /// <returns>A new <see cref="EventCentricAggregateFactoryTestResult"/>.</returns>
        public EventCentricAggregateFactoryTestResult Fail(object[] actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new EventCentricAggregateFactoryTestResult(
                this,
                TestResultState.Failed,
                new Optional<object[]>(actual),
                Optional<Exception>.Empty);
        }

        /// <summary>
        /// Returns a test result that indicates this specification has failed because an exception happened.
        /// </summary>
        /// <param name="actual">The actual exception</param>
        /// <returns>A new <see cref="EventCentricAggregateFactoryTestResult"/>.</returns>
        public EventCentricAggregateFactoryTestResult Fail(Exception actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new EventCentricAggregateFactoryTestResult(
                this,
                TestResultState.Failed,
                Optional<object[]>.Empty,
                new Optional<Exception>(actual));
        }
    }
}
