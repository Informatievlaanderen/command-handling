namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
    /// Represents an exception centric test specification, meaning that the outcome revolves around an exception as a result of executing a query method on an aggregate.
    /// </summary>
    public class ExceptionCentricAggregateQueryTestSpecification
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
        /// The query method to act upon.
        /// </summary>
        public Func<IAggregateRootEntity, object> When { get; }

        /// <summary>
        /// The expected exception to assert.
        /// </summary>
        public Exception Throws { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCentricAggregateQueryTestSpecification"/> class.
        /// </summary>
        /// <param name="sutFactory">The sut factory.</param>
        /// <param name="givens">The events to arrange.</param>
        /// <param name="when">The query method to act upon.</param>
        /// <param name="throws">The expected exception to assert.</param>
        public ExceptionCentricAggregateQueryTestSpecification(
            Func<IAggregateRootEntity> sutFactory, 
            object[] givens,
            Func<IAggregateRootEntity, object> when, 
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
        /// <returns>A new <see cref="ExceptionCentricAggregateQueryTestResult"/>.</returns>
        public ExceptionCentricAggregateQueryTestResult Pass()
            => new ExceptionCentricAggregateQueryTestResult(
                this,
                TestResultState.Passed,
                Optional<Exception>.Empty,
                Optional<object[]>.Empty,
                Optional<object>.Empty);

        /// <summary>
        /// Returns a test result that indicates this specification has failed because nothing happened.
        /// </summary>
        /// <returns>A new <see cref="ExceptionCentricAggregateQueryTestResult"/>.</returns>
        public ExceptionCentricAggregateQueryTestResult Fail()
            => new ExceptionCentricAggregateQueryTestResult(
                this,
                TestResultState.Failed,
                Optional<Exception>.Empty,
                Optional<object[]>.Empty,
                Optional<object>.Empty);

        /// <summary>
        /// Returns a test result that indicates this specification has failed because different things happened.
        /// </summary>
        /// <param name="actual">The actual events</param>
        /// <returns>A new <see cref="ExceptionCentricAggregateQueryTestResult"/>.</returns>
        public ExceptionCentricAggregateQueryTestResult Fail(object[] actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ExceptionCentricAggregateQueryTestResult(
                this,
                TestResultState.Failed,
                Optional<Exception>.Empty,
                new Optional<object[]>(actual),
                Optional<object>.Empty);
        }

        /// <summary>
        /// Returns a test result that indicates this specification has failed because an exception happened.
        /// </summary>
        /// <param name="actual">The actual exception</param>
        /// <returns>A new <see cref="ExceptionCentricAggregateQueryTestResult"/>.</returns>
        public ExceptionCentricAggregateQueryTestResult Fail(Exception actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ExceptionCentricAggregateQueryTestResult(
                this,
                TestResultState.Failed,
                new Optional<Exception>(actual),
                Optional<object[]>.Empty,
                Optional<object>.Empty);
        }

        /// <summary>
        /// Returns a test result that indicates this specification has failed because an exception happened.
        /// </summary>
        /// <param name="actual">The actual result</param>
        /// <returns>A new <see cref="ExceptionCentricAggregateQueryTestResult"/>.</returns>
        public ExceptionCentricAggregateQueryTestResult Fail(object actual)
            => new ExceptionCentricAggregateQueryTestResult(
                this,
                TestResultState.Failed,
                Optional<Exception>.Empty,
                Optional<object[]>.Empty,
                new Optional<object>(actual));
    }
}
