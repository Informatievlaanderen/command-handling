namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Command
{
    using System;
    using System.Linq;

    internal class AggregateCommandGivenStateBuilder<TAggregateRoot> : IAggregateCommandGivenStateBuilder<TAggregateRoot>
        where TAggregateRoot : IAggregateRootEntity
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;
        private readonly object[] _givens;

        public AggregateCommandGivenStateBuilder(Func<IAggregateRootEntity> sutFactory, object[] givens)
        {
            _sutFactory = sutFactory;
            _givens = givens;
        }

        public IAggregateCommandGivenStateBuilder<TAggregateRoot> Given(params object[] events)
        {
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            return new AggregateCommandGivenStateBuilder<TAggregateRoot>(_sutFactory, _givens.Concat(events).ToArray());
        }

        public IAggregateCommandWhenStateBuilder When(Action<TAggregateRoot> command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            return new AggregateCommandWhenStateBuilder(_sutFactory, _givens, root => command((TAggregateRoot) root));
        }
    }
}
