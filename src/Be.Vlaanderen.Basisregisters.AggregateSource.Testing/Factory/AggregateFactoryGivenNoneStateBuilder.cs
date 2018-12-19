namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Factory
{
    using System;

    internal class AggregateFactoryGivenNoneStateBuilder<TAggregateRoot> : IAggregateFactoryGivenNoneStateBuilder<TAggregateRoot>
        where TAggregateRoot : IAggregateRootEntity
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;

        public AggregateFactoryGivenNoneStateBuilder(Func<IAggregateRootEntity> sutFactory) => _sutFactory = sutFactory;

        public IAggregateFactoryWhenStateBuilder When<TAggregateRootResult>(
            Func<TAggregateRoot, TAggregateRootResult> factory) where TAggregateRootResult : IAggregateRootEntity
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return new AggregateFactoryWhenStateBuilder(_sutFactory, new object[0], root => factory((TAggregateRoot)root));
        }
    }
}
