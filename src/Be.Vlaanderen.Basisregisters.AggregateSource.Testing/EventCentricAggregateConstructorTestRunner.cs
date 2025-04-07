namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents an aggregate constructor test specification runner.
    /// </summary>
    public class EventCentricAggregateConstructorTestRunner : IEventCentricAggregateConstructorTestRunner
    {
        private readonly IEventComparer _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCentricAggregateConstructorTestRunner"/> class.
        /// </summary>
        /// <param name="comparer">The comparer to use when comparing events.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="comparer"/> is <c>null</c>.</exception>
        public EventCentricAggregateConstructorTestRunner(IEventComparer comparer)
            => _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

        /// <summary>
        /// Runs the specified test specification.
        /// </summary>
        /// <param name="specification">The test specification to run.</param>
        /// <returns>
        /// The result of running the test specification.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="specification"/> is <c>null</c>.</exception>
        public EventCentricAggregateConstructorTestResult Run(EventCentricAggregateConstructorTestSpecification specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            IAggregateRootEntity? sut = null;
            var result = Catch.Exception(() => sut = specification.SutFactory());

            if (result.HasValue)
                return specification.Fail(result.Value);

            var actualEvents = sut!.GetChanges().ToArray();

            return actualEvents!.SequenceEqual(specification.Thens, new WrappedEventComparerEqualityComparer(_comparer))
                ? specification.Pass()
                : specification.Fail(actualEvents);
        }
    }
}
