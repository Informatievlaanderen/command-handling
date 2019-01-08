namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore.Autofac
{
    using CommandHandling;
    using SqlStreamStore;
    using global::Autofac;

    public static class ContainerBuilderTestingExtensions
    {
        public static void UseAggregateSourceTesting(
            this ContainerBuilder builder,
            IFactComparer factComparer,
            IExceptionComparer exceptionComparer)
        {
            builder
                .RegisterType<StreamStoreFactRepository>()
                .AsImplementedInterfaces();

            builder
                .RegisterType<ReflectionBasedHandlerResolver>()
                .AsImplementedInterfaces();

            builder
                .RegisterInstance(factComparer)
                .AsImplementedInterfaces();

            builder
                .RegisterInstance(exceptionComparer)
                .AsImplementedInterfaces();

            builder
                .RegisterType<ExceptionCentricTestSpecificationRunner>()
                .AsImplementedInterfaces();

            builder.
                RegisterType<EventCentricTestSpecificationRunner>()
                .AsImplementedInterfaces();
        }
    }
}
