namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
    /// Represents an exception centric test specification, meaning that the outcome revolves around an exception.
    /// </summary>
    public class ExceptionCentricTestSpecification
    {
        /// <summary>
        /// The events to arrange.
        /// </summary>
        public Fact[] Givens { get; }

        /// <summary>
        /// The message to act upon.
        /// </summary>
        public object When { get; }

        /// <summary>
        /// The expected exception to assert.
        /// </summary>
        public Exception Throws { get; }

        /// <summary>
        /// Initializes a new <see cref="ExceptionCentricTestSpecification"/> instance.
        /// </summary>
        /// <param name="givens">The specification givens.</param>
        /// <param name="when">The specification when.</param>
        /// <param name="throws">The specification exception thrown.</param>
        public ExceptionCentricTestSpecification(Fact[] givens, object? when, Exception? throws)
        {
            Givens = givens ?? throw new ArgumentNullException(nameof(givens));
            When = when ?? throw new ArgumentNullException(nameof(when));
            Throws = throws ?? throw new ArgumentNullException(nameof(throws));
        }

        /// <summary>
        /// Returns a test result that indicates this specification has passed.
        /// </summary>
        /// <returns>A new <see cref="ExceptionCentricTestResult"/>.</returns>
        public ExceptionCentricTestResult Pass(Exception actual)
            => new ExceptionCentricTestResult(this, TestResultState.Passed, new Optional<Exception>(actual), Optional<Fact[]>.Empty);

        /// <summary>
        /// Returns a test result that indicates this specification has failed.
        /// </summary>
        /// <returns>A new <see cref="ExceptionCentricTestResult"/>.</returns>
        public ExceptionCentricTestResult Fail()
            => new ExceptionCentricTestResult(this, TestResultState.Failed, Optional<Exception>.Empty, Optional<Fact[]>.Empty);

        /// <summary>
        /// Returns a test result that indicates this specification has failed.
        /// </summary>
        /// <param name="actual">The actual exception thrown</param>
        /// <returns>A new <see cref="ExceptionCentricTestResult"/>.</returns>
        public ExceptionCentricTestResult Fail(Exception actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ExceptionCentricTestResult(this, TestResultState.Failed, new Optional<Exception>(actual), Optional<Fact[]>.Empty);
        }

        ///// <summary>
        ///// Returns a test result that indicates this specification has failed.
        ///// </summary>
        ///// <param name="actual">The actual exception thrown</param>
        ///// <returns>A new <see cref="ExceptionCentricTestResult"/>.</returns>
        //public ExceptionCentricTestResult Fail<TException>()
        //    where TException : Exception, new()
        //{
        //    return new ExceptionCentricTestResult(this, TestResultState.Failed, new Optional<Exception>(new TException()), Optional<Fact[]>.Empty);
        //}

        /// <summary>
        /// Returns a test result that indicates this specification has failed because different things happened.
        /// </summary>
        /// <param name="actual">The actual events</param>
        /// <returns>A new <see cref="ExceptionCentricTestResult"/>.</returns>
        public ExceptionCentricTestResult Fail(Fact[] actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            return new ExceptionCentricTestResult(this, TestResultState.Failed, Optional<Exception>.Empty, new Optional<Fact[]>(actual));
        }

        /// <summary>
        /// Determines whether the specified <see cref="ExceptionCentricTestSpecification" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="ExceptionCentricTestSpecification" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="ExceptionCentricTestSpecification" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(ExceptionCentricTestSpecification other) => Equals(Givens, other.Givens) &&  Equals(When, other.When) && Equals(Throws, other.Throws);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            return Equals((ExceptionCentricTestSpecification) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() => Givens.GetHashCode() ^ When.GetHashCode() ^ Throws.GetHashCode();
    }
}
