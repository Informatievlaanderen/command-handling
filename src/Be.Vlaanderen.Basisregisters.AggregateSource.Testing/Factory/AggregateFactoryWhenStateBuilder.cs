namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Factory
{
    using System;

    internal class AggregateFactoryWhenStateBuilder : IAggregateFactoryWhenStateBuilder
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;
        private readonly object[] _givens;
        private readonly Func<IAggregateRootEntity, IAggregateRootEntity> _when;

        public AggregateFactoryWhenStateBuilder(
            Func<IAggregateRootEntity> sutFactory,
            object[] givens,
            Func<IAggregateRootEntity,
            IAggregateRootEntity> when)
        {
            _sutFactory = sutFactory;
            _givens = givens;
            _when = when;
        }

        public IAggregateFactoryThenNoneStateBuilder ThenNone()
            => new AggregateFactoryThenNoneStateBuilder(_sutFactory, _givens, _when);

        public IAggregateFactoryThenStateBuilder Then(params object[] events)
        {
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            return new AggregateFactoryThenStateBuilder(_sutFactory, _givens, _when, events);
        }

        public IAggregateFactoryThrowStateBuilder Throws(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return new AggregateFactoryThrowStateBuilder(_sutFactory, _givens, _when, exception);
        }
    }
}
