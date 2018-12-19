namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Factory
{
    using System;

    internal class AggregateFactoryThrowStateBuilder : IAggregateFactoryThrowStateBuilder
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;
        private readonly object[] _givens;
        private readonly Func<IAggregateRootEntity, IAggregateRootEntity> _when;
        private readonly Exception _throws;

        public AggregateFactoryThrowStateBuilder(
            Func<IAggregateRootEntity> sutFactory,
            object[] givens,
            Func<IAggregateRootEntity, IAggregateRootEntity> when,
            Exception throws)
        {
            _sutFactory = sutFactory;
            _givens = givens;
            _when = when;
            _throws = throws;
        }

        public ExceptionCentricAggregateFactoryTestSpecification Build()
            => new ExceptionCentricAggregateFactoryTestSpecification(_sutFactory, _givens, _when, _throws);
    }
}
