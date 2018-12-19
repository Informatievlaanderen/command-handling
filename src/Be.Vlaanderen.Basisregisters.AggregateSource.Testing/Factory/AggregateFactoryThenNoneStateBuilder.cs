namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Factory
{
    using System;

    internal class AggregateFactoryThenNoneStateBuilder : IAggregateFactoryThenNoneStateBuilder
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;
        private readonly object[] _givens;
        private readonly Func<IAggregateRootEntity, IAggregateRootEntity> _when;

        public AggregateFactoryThenNoneStateBuilder(Func<IAggregateRootEntity> sutFactory, object[] givens,
            Func<IAggregateRootEntity, IAggregateRootEntity> when)
        {
            _sutFactory = sutFactory;
            _givens = givens;
            _when = when;
        }

        public EventCentricAggregateFactoryTestSpecification Build()
            => new EventCentricAggregateFactoryTestSpecification(_sutFactory, _givens, _when, new object[0]);
    }
}
