namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Query
{
    using System;
    using System.Linq;

    internal class AggregateQueryGivenStateBuilder<TAggregateRoot> : IAggregateQueryGivenStateBuilder<TAggregateRoot>
        where TAggregateRoot : IAggregateRootEntity
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;
        private readonly object[] _givens;

        public AggregateQueryGivenStateBuilder(Func<IAggregateRootEntity> sutFactory, object[] givens)
        {
            _sutFactory = sutFactory;
            _givens = givens;
        }

        public IAggregateQueryGivenStateBuilder<TAggregateRoot> Given(params object[] events)
        {
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            return new AggregateQueryGivenStateBuilder<TAggregateRoot>(_sutFactory, _givens.Concat(events).ToArray());
        }

        public IAggregateQueryWhenStateBuilder<TResult> When<TResult>(Func<TAggregateRoot, TResult> query)
        where TResult : notnull
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return new AggregateQueryWhenStateBuilder<TResult>(_sutFactory, _givens, root => query((TAggregateRoot) root));
        }
    }
}
