namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
    /// Represents an result-centric test specification, meaning that the outcome revolves around the result of executing a query method.
    /// </summary>
    public class ResultCentricAggregateQueryTestSpecification
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
        /// The expected result to assert.
        /// </summary>
        public object Then { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultCentricAggregateQueryTestSpecification"/> class.
        /// </summary>
        /// <param name="sutFactory">The sut factory.</param>
        /// <param name="givens">The events to arrange.</param>
        /// <param name="when">The query method to act upon.</param>
        /// <param name="then">The events to assert.</param>
        public ResultCentricAggregateQueryTestSpecification(
            Func<IAggregateRootEntity> sutFactory, 
            object[] givens,
            Func<IAggregateRootEntity, object> when, 
            object then)
        {
            SutFactory = sutFactory ?? throw new ArgumentNullException(nameof(sutFactory));
            Givens = givens ?? throw new ArgumentNullException(nameof(givens));
            When = when ?? throw new ArgumentNullException(nameof(when));
            Then = then;
        }

        /// <summary>
        /// Returns a test result that indicates this specification has passed.
        /// </summary>
        /// <returns>A new <see cref="EventCentricAggregateCommandTestResult"/>.</returns>
        public ResultCentricAggregateQueryTestResult Pass()
        {
            return new ResultCentricAggregateQueryTestResult(
                this,
                TestResultState.Passed,
                Optional<object>.Empty,
                Optional<Exception>.Empty,
                Optional<object[]>.Empty);
        }

        /// <summary>
        /// Returns a test result that indicates this specification has failed because different things happened.
        /// </summary>
        /// <param name="actual">The actual events</param>
        /// <returns>A new <see cref="ResultCentricAggregateQueryTestResult"/>.</returns>
        public ResultCentricAggregateQueryTestResult Fail(object[] actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ResultCentricAggregateQueryTestResult(
                this,
                TestResultState.Failed,
                Optional<object>.Empty,
                Optional<Exception>.Empty,
                new Optional<object[]>(actual));
        }

        /// <summary>
        /// Returns a test result that indicates this specification has failed because an exception happened.
        /// </summary>
        /// <param name="actual">The actual exception</param>
        /// <returns>A new <see cref="ResultCentricAggregateQueryTestResult"/>.</returns>
        public ResultCentricAggregateQueryTestResult Fail(Exception actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ResultCentricAggregateQueryTestResult(
                this,
                TestResultState.Failed,
                Optional<object>.Empty,
                new Optional<Exception>(actual),
                Optional<object[]>.Empty);
        }

        /// <summary>
        /// Returns a test result that indicates this specification has failed because a different query result was returned.
        /// </summary>
        /// <param name="actual">The actual exception</param>
        /// <returns>A new <see cref="ResultCentricAggregateQueryTestResult"/>.</returns>
        public ResultCentricAggregateQueryTestResult Fail(object actual)
        {
            return new ResultCentricAggregateQueryTestResult(
                this,
                TestResultState.Failed,
                new Optional<object>(actual),
                Optional<Exception>.Empty,
                Optional<object[]>.Empty);
        }
    }
}
