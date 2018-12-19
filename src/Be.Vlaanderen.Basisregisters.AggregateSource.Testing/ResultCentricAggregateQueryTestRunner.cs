namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents an aggregate query test specification runner.
    /// </summary>
    public class ResultCentricAggregateQueryTestRunner : IResultCentricAggregateQueryTestRunner
    {
        private readonly IResultComparer _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultCentricAggregateQueryTestRunner"/> class.
        /// </summary>
        /// <param name="comparer">The comparer to use when comparing events.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="comparer"/> is <c>null</c>.</exception>
        public ResultCentricAggregateQueryTestRunner(IResultComparer comparer)
            => _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

        /// <summary>
        /// Runs the specified test specification.
        /// </summary>
        /// <param name="specification">The test specification to run.</param>
        /// <returns>
        /// The result of running the test specification.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="specification"/> is <c>null</c>.</exception>
        public ResultCentricAggregateQueryTestResult Run(ResultCentricAggregateQueryTestSpecification specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));

            var sut = specification.SutFactory();
            sut.Initialize(specification.Givens);

            object queryResult = null;
            var result = Catch.Exception(() => queryResult = specification.When(sut));

            if (result.HasValue)
                return specification.Fail(result.Value);

            if (_comparer.Compare(queryResult, specification.Then).Any())
                return specification.Fail(queryResult);

            return sut.HasChanges()
                ? specification.Fail(sut.GetChanges().ToArray())
                : specification.Pass();
        }
    }
}
