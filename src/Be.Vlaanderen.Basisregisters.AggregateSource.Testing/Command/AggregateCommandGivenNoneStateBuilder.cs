namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Command
{
    using System;

    internal class AggregateCommandGivenNoneStateBuilder<TAggregateRoot> : IAggregateCommandGivenNoneStateBuilder<TAggregateRoot> 
        where TAggregateRoot : IAggregateRootEntity
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;

        public AggregateCommandGivenNoneStateBuilder(Func<IAggregateRootEntity> sutFactory) => _sutFactory = sutFactory;

        public IAggregateCommandWhenStateBuilder When(Action<TAggregateRoot> command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            return new AggregateCommandWhenStateBuilder(_sutFactory, new object[0], root => command((TAggregateRoot)root));
        }
    }
}
