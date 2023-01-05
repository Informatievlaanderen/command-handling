namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore.Microsoft
{
    using CommandHandling;
    using SqlStreamStore;
    using global::Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionTestingExtensions
    {
        public static void UseAggregateSourceTesting(
            this IServiceCollection services,
            IFactComparer factComparer,
            IExceptionComparer exceptionComparer)
        {
            services.AddTransient<IFactReader, StreamStoreFactRepository>();
            services.AddTransient<IFactWriter, StreamStoreFactRepository>();

            services.AddTransient<IHandlerResolver, ReflectionBasedHandlerResolver>();

            services.AddTransient(_ => factComparer);
            services.AddTransient(_ => exceptionComparer);

            services.AddTransient<IExceptionCentricTestSpecificationRunner, ExceptionCentricTestSpecificationRunner>();
            services.AddTransient<IEventCentricTestSpecificationRunner, EventCentricTestSpecificationRunner>();
        }
    }
}
