namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
    /// The result of an exception centric constructor command test specification.
    /// </summary>
    public class ExceptionCentricAggregateConstructorTestResult
    {
        private readonly TestResultState _state;

        /// <summary>
        /// Gets the test specification associated with this result.
        /// </summary>
        /// <value>
        /// The test specification.
        /// </value>
        public ExceptionCentricAggregateConstructorTestSpecification Specification { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ExceptionCentricAggregateConstructorTestResult"/> has passed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if passed; otherwise, <c>false</c>.
        /// </value>
        public bool Passed => _state == TestResultState.Passed;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ExceptionCentricAggregateConstructorTestResult"/> has failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
        public bool Failed => _state == TestResultState.Failed;

        /// <summary>
        /// Gets the exception that happened instead of the expected one, or empty if passed.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Optional<Exception> ButException { get; }

        /// <summary>
        /// Gets the events that happened instead of the expected exception, or empty if passed.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public Optional<object[]> ButEvents { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCentricTestResult"/> class.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="state">The state.</param>
        /// <param name="actualException">The actual exception.</param>
        /// <param name="actualEvents">The actual events.</param>
        internal ExceptionCentricAggregateConstructorTestResult(
            ExceptionCentricAggregateConstructorTestSpecification specification,
            TestResultState state,
            Optional<Exception> actualException,
            Optional<object[]> actualEvents)
        {
            Specification = specification;
            _state = state;
            ButException = actualException;
            ButEvents = actualEvents;
        }
    }
}
