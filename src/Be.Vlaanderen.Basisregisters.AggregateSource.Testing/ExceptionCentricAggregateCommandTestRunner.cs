namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents an aggregate command test specification runner.
    /// </summary>
    public class ExceptionCentricAggregateCommandTestRunner : IExceptionCentricAggregateCommandTestRunner
    {
        private readonly IExceptionComparer _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCentricAggregateCommandTestRunner"/> class.
        /// </summary>
        /// <param name="comparer">The comparer to use when comparing exceptions.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="comparer"/> is <c>null</c>.</exception>
        public ExceptionCentricAggregateCommandTestRunner(IExceptionComparer comparer)
            => _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

        /// <summary>
        /// Runs the specified test specification.
        /// </summary>
        /// <param name="specification">The test specification to run.</param>
        /// <returns>
        /// The result of running the test specification.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="specification"/> is <c>null</c>.</exception>
        public ExceptionCentricAggregateCommandTestResult Run(ExceptionCentricAggregateCommandTestSpecification specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            var sut = specification.SutFactory();
            sut.Initialize(specification.Givens);

            var result = Catch.Exception(() => specification.When(sut));

            if (!result.HasValue)
                return sut.HasChanges()
                    ? specification.Fail(sut.GetChanges().ToArray())
                    : specification.Fail();

            var actualException = result.Value;

            return _comparer.Compare(actualException, specification.Throws).Any()
                ? specification.Fail(actualException)
                : specification.Pass();
        }
    }
}
