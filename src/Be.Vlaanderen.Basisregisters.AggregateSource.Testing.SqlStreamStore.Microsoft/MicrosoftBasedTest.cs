namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore.Microsoft
{
    using System;
    using AggregateSource.SqlStreamStore.Microsoft;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Comparers;
    using DependencyInjection;
    using global::Microsoft.Extensions.DependencyInjection;
    using global::Microsoft.Extensions.Logging;
    using KellermanSoftware.CompareNetObjects;
    using Xunit.Abstractions;

    public abstract class MicrosoftBasedTest
    {
        private readonly Lazy<IServiceProvider> _serviceProvider;

        protected IServiceProvider Container => _serviceProvider.Value;

        protected IExceptionCentricTestSpecificationRunner ExceptionCentricTestSpecificationRunner => _serviceProvider.Value.GetRequiredService<IExceptionCentricTestSpecificationRunner>();

        protected IEventCentricTestSpecificationRunner EventCentricTestSpecificationRunner => _serviceProvider.Value.GetRequiredService<IEventCentricTestSpecificationRunner>();

        protected IFactComparer FactComparer => _serviceProvider.Value.GetRequiredService<IFactComparer>();

        protected IExceptionComparer ExceptionComparer => _serviceProvider.Value.GetRequiredService<IExceptionComparer>();

        protected ILogger Logger => _serviceProvider.Value.GetRequiredService<ILogger>();

        protected MicrosoftBasedTest(
            ITestOutputHelper testOutputHelper,
            Action<IServiceCollection> registerFunc = null)
        {
            _serviceProvider = new Lazy<IServiceProvider>(() =>
            {
                var serviceCollection = new ServiceCollection();

                ConfigureEventHandling(serviceCollection);
                ConfigureCommandHandling(serviceCollection);
                serviceCollection.RegisterModule(new SqlStreamStoreModule());

                serviceCollection.UseAggregateSourceTesting(CreateFactComparer(), CreateExceptionComparer());

                serviceCollection.AddTransient(_ => testOutputHelper);
                serviceCollection.AddTransient<XUnitLogger>();

                registerFunc?.Invoke(serviceCollection);

                return serviceCollection.BuildServiceProvider();
            });
        }

        protected abstract void ConfigureCommandHandling(IServiceCollection services);
        protected abstract void ConfigureEventHandling(IServiceCollection services);

        protected virtual IFactComparer CreateFactComparer()
        {
            var comparer = new CompareLogic();
            return new CompareNetObjectsBasedFactComparer(comparer);
        }

        protected virtual IExceptionComparer CreateExceptionComparer()
        {
            var comparer = new CompareLogic();
            comparer.Config.MembersToIgnore.Add("Source");
            comparer.Config.MembersToIgnore.Add("StackTrace");
            comparer.Config.MembersToIgnore.Add("TargetSite");
            return new CompareNetObjectsBasedExceptionComparer(comparer);
        }

        protected void Assert(IExceptionCentricTestSpecificationBuilder builder)
            => builder.Assert(ExceptionCentricTestSpecificationRunner, ExceptionComparer, Logger);

        protected void Assert(IEventCentricTestSpecificationBuilder builder)
            => builder.Assert(EventCentricTestSpecificationRunner, FactComparer, Logger);
    }
}
