namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
    /// Represents an exception centric test specification, meaning that the outcome revolves around an exception as a result of executing a factory method on an aggregate.
    /// </summary>
    public class ExceptionCentricAggregateFactoryTestSpecification
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
        /// The expected exception to assert.
        /// </summary>
        public Exception Throws { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCentricAggregateFactoryTestSpecification"/> class.
        /// </summary>
        /// <param name="sutFactory">The sut factory.</param>
        /// <param name="givens">The events to arrange.</param>
        /// <param name="when">The factory method to act upon.</param>
        /// <param name="throws">The expected exception to assert.</param>
        public ExceptionCentricAggregateFactoryTestSpecification(
            Func<IAggregateRootEntity> sutFactory,
            object[] givens,
            Func<IAggregateRootEntity, IAggregateRootEntity> when,
            Exception throws)
        {
            SutFactory = sutFactory ?? throw new ArgumentNullException(nameof(sutFactory));
            Givens = givens ?? throw new ArgumentNullException(nameof(givens));
            When = when ?? throw new ArgumentNullException(nameof(when));
            Throws = throws ?? throw new ArgumentNullException(nameof(throws));
        }

        /// <summary>
        /// Returns a test result that indicates this specification has passed.
        /// </summary>
        /// <returns>A new <see cref="ExceptionCentricAggregateFactoryTestResult"/>.</returns>
        public ExceptionCentricAggregateFactoryTestResult Pass()
            => new ExceptionCentricAggregateFactoryTestResult(
                this,
                TestResultState.Passed,
                Optional<Exception>.Empty,
                Optional<object[]>.Empty);

        /// <summary>
        /// Returns a test result that indicates this specification has failed because nothing happened.
        /// </summary>
        /// <returns>A new <see cref="EventCentricAggregateFactoryTestResult"/>.</returns>
        public ExceptionCentricAggregateFactoryTestResult Fail()
            => new ExceptionCentricAggregateFactoryTestResult(
                this,
                TestResultState.Failed,
                Optional<Exception>.Empty,
                Optional<object[]>.Empty);

        /// <summary>
        /// Returns a test result that indicates this specification has failed because different things happened.
        /// </summary>
        /// <param name="actual">The actual events</param>
        /// <returns>A new <see cref="ExceptionCentricAggregateFactoryTestResult"/>.</returns>
        public ExceptionCentricAggregateFactoryTestResult Fail(object[] actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ExceptionCentricAggregateFactoryTestResult(
                this,
                TestResultState.Failed,
                Optional<Exception>.Empty,
                new Optional<object[]>(actual));
        }

        /// <summary>
        /// Returns a test result that indicates this specification has failed because an exception happened.
        /// </summary>
        /// <param name="actual">The actual exception</param>
        /// <returns>A new <see cref="ExceptionCentricAggregateFactoryTestResult"/>.</returns>
        public ExceptionCentricAggregateFactoryTestResult Fail(Exception actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ExceptionCentricAggregateFactoryTestResult(
                this,
                TestResultState.Failed,
                new Optional<Exception>(actual),
                Optional<object[]>.Empty);
        }
    }
}
