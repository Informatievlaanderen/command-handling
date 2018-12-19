namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
    /// The result of a result centric aggregate query test specification run.
    /// </summary>
    public class ResultCentricAggregateQueryTestResult
    {
        private readonly TestResultState _state;

        /// <summary>
        /// Gets the test specification associated with this result.
        /// </summary>
        /// <value>
        /// The test specification.
        /// </value>
        public ResultCentricAggregateQueryTestSpecification Specification { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ResultCentricAggregateQueryTestResult"/> has passed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if passed; otherwise, <c>false</c>.
        /// </value>
        public bool Passed => _state == TestResultState.Passed;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ResultCentricAggregateQueryTestResult"/> has failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
        public bool Failed => _state == TestResultState.Failed;

        /// <summary>
        /// Gets the result that happened instead of the expected one, or empty if passed.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public Optional<object> ButResult { get; }

        /// <summary>
        /// Gets the exception that happened instead of the expected events, or empty if passed.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Optional<Exception> ButException { get; }

        /// <summary>
        /// Gets the events that happened instead of the expected ones, or empty if passed.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public Optional<object[]> ButEvents { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultCentricAggregateQueryTestResult"/> class.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="state">The state.</param>
        /// <param name="actualResult">The actual result.</param>
        /// <param name="actualException">The actual exception.</param>
        /// <param name="actualEvents">The actual events.</param>
        internal ResultCentricAggregateQueryTestResult(
            ResultCentricAggregateQueryTestSpecification specification,
            TestResultState state,
            Optional<object> actualResult,
            Optional<Exception> actualException,
            Optional<object[]> actualEvents)
        {
            Specification = specification;
            _state = state;
            ButResult = actualResult;
            ButException = actualException;
            ButEvents = actualEvents;
        }
    }
}
