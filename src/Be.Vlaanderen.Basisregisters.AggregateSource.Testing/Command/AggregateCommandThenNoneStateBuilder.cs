namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Command
{
    using System;

    internal class AggregateCommandThenNoneStateBuilder : IAggregateCommandThenNoneStateBuilder
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;
        private readonly object[] _givens;
        private readonly Action<IAggregateRootEntity> _when;

        public AggregateCommandThenNoneStateBuilder(Func<IAggregateRootEntity> sutFactory, object[] givens, Action<IAggregateRootEntity> when)
        {
            _sutFactory = sutFactory;
            _givens = givens;
            _when = when;
        }

        public EventCentricAggregateCommandTestSpecification Build()
            => new EventCentricAggregateCommandTestSpecification(_sutFactory, _givens, _when, new object[0]);
    }
}
