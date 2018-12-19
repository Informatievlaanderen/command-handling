namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Query
{
    using System;

    internal class AggregateQueryThenStateBuilder : IAggregateQueryThenStateBuilder
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;
        private readonly object[] _givens;
        private readonly Func<IAggregateRootEntity, object> _when;
        private readonly object _then;

        public AggregateQueryThenStateBuilder(
            Func<IAggregateRootEntity> sutFactory,
            object[] givens,
            Func<IAggregateRootEntity, object> when,
            object then)
        {
            _sutFactory = sutFactory;
            _givens = givens;
            _when = when;
            _then = then;
        }

        public ResultCentricAggregateQueryTestSpecification Build()
            => new ResultCentricAggregateQueryTestSpecification(_sutFactory, _givens, _when, _then);
    }
}
