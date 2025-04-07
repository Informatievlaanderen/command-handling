namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using Query;

    /// <summary>
    /// A given-when-then test specification bootstrapper for testing an aggregate query, i.e. a method on the aggregate that returns a value (but is not a factory of another aggregate).
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of aggregate root entity under test.</typeparam>
    public class QueryScenarioFor<TAggregateRoot> : IAggregateQueryInitialStateBuilder<TAggregateRoot>
        where TAggregateRoot : IAggregateRootEntity
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryScenarioFor{TAggregateRoot}"/> class.
        /// </summary>
        /// <param name="sut">The sut.</param>
        public QueryScenarioFor(TAggregateRoot sut)
            : this(() => sut) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryScenarioFor{TAggregateRoot}"/> class.
        /// </summary>
        /// <param name="sutFactory">The sut factory.</param>
        public QueryScenarioFor(Func<TAggregateRoot> sutFactory)
        {
            if (sutFactory == null)
                throw new ArgumentNullException(nameof(sutFactory));

            _sutFactory = () => sutFactory();
        }

        /// <summary>
        /// Given no events occured.
        /// </summary>
        /// <returns>A builder continuation.</returns>
        public IAggregateQueryGivenNoneStateBuilder<TAggregateRoot> GivenNone()
            => new AggregateQueryGivenNoneStateBuilder<TAggregateRoot>(_sutFactory);

        /// <summary>
        /// Given the following events occured.
        /// </summary>
        /// <param name="events">The events that occurred.</param>
        /// <returns>A builder continuation.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="events"/> are <c>null</c>.</exception>
        public IAggregateQueryGivenStateBuilder<TAggregateRoot> Given(params object[] events)
        {
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            return new AggregateQueryGivenStateBuilder<TAggregateRoot>(_sutFactory, events);
        }

        /// <summary>
        /// When a query occurs.
        /// </summary>
        /// <param name="query">The query method invocation on the sut.</param>
        /// <returns>A builder continuation.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="query"/> is <c>null</c>.</exception>
        public IAggregateQueryWhenStateBuilder<TResult> When<TResult>(Func<TAggregateRoot, TResult> query)
        where TResult : notnull
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return new AggregateQueryWhenStateBuilder<TResult>(_sutFactory, new object[0], root => query((TAggregateRoot) root));
        }
    }
}
