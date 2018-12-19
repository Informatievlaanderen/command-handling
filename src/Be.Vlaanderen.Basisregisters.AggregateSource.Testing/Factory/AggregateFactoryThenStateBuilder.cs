namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Factory
{
    using System;
    using System.Linq;

    internal class AggregateFactoryThenStateBuilder : IAggregateFactoryThenStateBuilder
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;
        private readonly object[] _givens;
        private readonly Func<IAggregateRootEntity, IAggregateRootEntity> _when;
        private readonly object[] _thens;

        public AggregateFactoryThenStateBuilder(
            Func<IAggregateRootEntity> sutFactory,
            object[] givens,
            Func<IAggregateRootEntity, IAggregateRootEntity> when,
            object[] thens)
        {
            _sutFactory = sutFactory;
            _givens = givens;
            _when = when;
            _thens = thens;
        }

        public IAggregateFactoryThenStateBuilder Then(params object[] events)
        {
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            return new AggregateFactoryThenStateBuilder(_sutFactory, _givens, _when, _thens.Concat(events).ToArray());
        }

        public EventCentricAggregateFactoryTestSpecification Build()
            => new EventCentricAggregateFactoryTestSpecification(_sutFactory, _givens, _when, _thens);
    }
}
