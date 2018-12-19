namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Query
{
    using System;

    internal class AggregateQueryGivenNoneStateBuilder<TAggregateRoot> : IAggregateQueryGivenNoneStateBuilder<TAggregateRoot> where TAggregateRoot : IAggregateRootEntity
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;

        public AggregateQueryGivenNoneStateBuilder(Func<IAggregateRootEntity> sutFactory) => _sutFactory = sutFactory;

        public IAggregateQueryWhenStateBuilder<TResult> When<TResult>(Func<TAggregateRoot, TResult> query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return new AggregateQueryWhenStateBuilder<TResult>(_sutFactory, new object[0], root => query((TAggregateRoot)root));
        }
    }
}
