namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
	/// Represents an exception centric constructor test specification, meaning that the outcome revolves around an exception as a result of constructing an aggregate.
	/// </summary>
	public class ExceptionCentricAggregateConstructorTestSpecification
	{
	    /// <summary>
	    /// Gets the sut factory.
	    /// </summary>
	    /// <value>
	    /// The sut factory.
	    /// </value>
	    public Func<IAggregateRootEntity> SutFactory { get; }

	    /// <summary>
	    /// The expected exception to assert.
	    /// </summary>
	    public Exception Throws { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCentricAggregateConstructorTestSpecification"/> class.
        /// </summary>
        /// <param name="sutFactory">The sut factory.</param>
        /// <param name="throws">The expected exception to assert.</param>
        public ExceptionCentricAggregateConstructorTestSpecification(Func<IAggregateRootEntity> sutFactory, Exception throws)
		{
            SutFactory = sutFactory ?? throw new ArgumentNullException("sutFactory");
			Throws = throws ?? throw new ArgumentNullException("throws");
		}

	    /// <summary>
	    /// Returns a test result that indicates this specification has passed.
	    /// </summary>
	    /// <returns>A new <see cref="ExceptionCentricAggregateConstructorTestResult"/>.</returns>
	    public ExceptionCentricAggregateConstructorTestResult Pass()
	        => new ExceptionCentricAggregateConstructorTestResult(
	            this,
	            TestResultState.Passed,
	            Optional<Exception>.Empty,
	            Optional<object[]>.Empty);

	    /// <summary>
	    /// Returns a test result that indicates this specification has failed because nothing happened.
	    /// </summary>
	    /// <returns>A new <see cref="ExceptionCentricAggregateConstructorTestResult"/>.</returns>
	    public ExceptionCentricAggregateConstructorTestResult Fail()
	        => new ExceptionCentricAggregateConstructorTestResult(
	            this,
	            TestResultState.Failed,
	            Optional<Exception>.Empty,
	            Optional<object[]>.Empty);

	    /// <summary>
        /// Returns a test result that indicates this specification has failed because different things happened.
        /// </summary>
        /// <param name="actual">The actual events</param>
        /// <returns>A new <see cref="ExceptionCentricAggregateConstructorTestResult"/>.</returns>
        public ExceptionCentricAggregateConstructorTestResult Fail(object[] actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ExceptionCentricAggregateConstructorTestResult(
                this,
                TestResultState.Failed,
                Optional<Exception>.Empty,
                new Optional<object[]>(actual));
        }

        /// <summary>
        /// Returns a test result that indicates this specification has failed because an exception happened.
        /// </summary>
        /// <param name="actual">The actual exception</param>
        /// <returns>A new <see cref="ExceptionCentricAggregateConstructorTestResult"/>.</returns>
        public ExceptionCentricAggregateConstructorTestResult Fail(Exception actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ExceptionCentricAggregateConstructorTestResult(
                this,
                TestResultState.Failed,
                new Optional<Exception>(actual),
                Optional<object[]>.Empty);
        }
	}
}
