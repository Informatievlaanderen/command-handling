namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Command
{
    using System;

    internal class AggregateCommandThrowStateBuilder : IAggregateCommandThrowStateBuilder
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;
        private readonly object[] _givens;
        private readonly Action<IAggregateRootEntity> _when;
        private readonly Exception _throws;

        public AggregateCommandThrowStateBuilder(
            Func<IAggregateRootEntity> sutFactory,
            object[] givens,
            Action<IAggregateRootEntity> when,
            Exception throws)
        {
            _sutFactory = sutFactory;
            _givens = givens;
            _when = when;
            _throws = throws;
        }

        public ExceptionCentricAggregateCommandTestSpecification Build()
            => new ExceptionCentricAggregateCommandTestSpecification(_sutFactory, _givens, _when, _throws);
    }
}
