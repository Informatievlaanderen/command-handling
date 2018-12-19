namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
    /// The result of an event centric aggregate factory test specification run.
    /// </summary>
    public class EventCentricAggregateFactoryTestResult
    {
        private readonly TestResultState _state;

        /// <summary>
        /// Gets the test specification associated with this result.
        /// </summary>
        /// <value>
        /// The test specification.
        /// </value>
        public EventCentricAggregateFactoryTestSpecification Specification { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="EventCentricTestResult"/> has passed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if passed; otherwise, <c>false</c>.
        /// </value>
        public bool Passed => _state == TestResultState.Passed;

        /// <summary>
        /// Gets a value indicating whether this <see cref="EventCentricTestResult"/> has failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
        public bool Failed => _state == TestResultState.Failed;

        /// <summary>
        /// Gets the events that happened instead of the expected ones, or empty if passed.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public Optional<object[]> ButEvents { get; }

        /// <summary>
        /// Gets the exception that happened instead of the expected events, or empty if passed.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Optional<Exception> ButException { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCentricTestResult"/> class.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="state">The state.</param>
        /// <param name="actualEvents">The actual events.</param>
        /// <param name="actualException">The actual exception.</param>
        internal EventCentricAggregateFactoryTestResult(
            EventCentricAggregateFactoryTestSpecification specification, 
            TestResultState state,
            Optional<object[]> actualEvents, 
            Optional<Exception> actualException)
        {
            Specification = specification;
            _state = state;
            ButEvents = actualEvents;
            ButException = actualException;
        }
    }
}
