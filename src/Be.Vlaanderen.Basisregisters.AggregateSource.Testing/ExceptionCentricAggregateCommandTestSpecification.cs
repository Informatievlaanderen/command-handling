namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
    /// Represents an exception centric test specification, meaning that the outcome revolves around an exception as a result of executing a command method on an aggregate.
    /// </summary>
    public class ExceptionCentricAggregateCommandTestSpecification
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
        /// The expected exception to assert.
        /// </summary>
        public Exception Throws { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCentricAggregateCommandTestSpecification"/> class.
        /// </summary>
        /// <param name="sutFactory">The sut factory.</param>
        /// <param name="givens">The events to arrange.</param>
        /// <param name="when">The command method to act upon.</param>
        /// <param name="throws">The expected exception to assert.</param>
        public ExceptionCentricAggregateCommandTestSpecification(
            Func<IAggregateRootEntity> sutFactory,
            object[] givens,
            Action<IAggregateRootEntity> when,
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
        /// <returns>A new <see cref="ExceptionCentricAggregateCommandTestResult"/>.</returns>
        public ExceptionCentricAggregateCommandTestResult Pass()
            => new ExceptionCentricAggregateCommandTestResult(
                this,
                TestResultState.Passed,
                Optional<Exception>.Empty,
                Optional<object[]>.Empty);

        /// <summary>
        /// Returns a test result that indicates this specification has failed because nothing happened.
        /// </summary>
        /// <returns>A new <see cref="ExceptionCentricAggregateCommandTestResult"/>.</returns>
        public ExceptionCentricAggregateCommandTestResult Fail()
            => new ExceptionCentricAggregateCommandTestResult(
                this,
                TestResultState.Failed,
                Optional<Exception>.Empty,
                Optional<object[]>.Empty);

        /// <summary>
        /// Returns a test result that indicates this specification has failed because different things happened.
        /// </summary>
        /// <param name="actual">The actual events</param>
        /// <returns>A new <see cref="ExceptionCentricAggregateCommandTestResult"/>.</returns>
        public ExceptionCentricAggregateCommandTestResult Fail(object[] actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ExceptionCentricAggregateCommandTestResult(
                this,
                TestResultState.Failed,
                Optional<Exception>.Empty,
                new Optional<object[]>(actual));
        }

        /// <summary>
        /// Returns a test result that indicates this specification has failed because an exception happened.
        /// </summary>
        /// <param name="actual">The actual exception</param>
        /// <returns>A new <see cref="ExceptionCentricAggregateCommandTestResult"/>.</returns>
        public ExceptionCentricAggregateCommandTestResult Fail(Exception actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ExceptionCentricAggregateCommandTestResult(
                this,
                TestResultState.Failed,
                new Optional<Exception>(actual),
                Optional<object[]>.Empty);
        }
    }
}
