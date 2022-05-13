namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.SqlStreamStore.Autofac
{
    using System;
    using AggregateSource.SqlStreamStore.Autofac;
    using Testing;
    using Comparers;
    using global::Autofac;
    using KellermanSoftware.CompareNetObjects;
    using Microsoft.Extensions.Logging;
    using Xunit.Abstractions;

    public abstract class AutofacBasedTest
    {
        private readonly Lazy<IContainer> _container;

        protected IContainer Container => _container.Value;

        protected IExceptionCentricTestSpecificationRunner ExceptionCentricTestSpecificationRunner => Container.Resolve<IExceptionCentricTestSpecificationRunner>();

        protected IEventCentricTestSpecificationRunner EventCentricTestSpecificationRunner => Container.Resolve<IEventCentricTestSpecificationRunner>();

        protected IExpectedFactComparer FactComparer => Container.Resolve<IExpectedFactComparer>();

        protected IExceptionComparer ExceptionComparer => Container.Resolve<IExceptionComparer>();

        protected ILogger Logger => Container.Resolve<ILogger>();

        protected AutofacBasedTest(
            ITestOutputHelper testOutputHelper,
            Action<ContainerBuilder> registerFunc = null)
        {
            _container = new Lazy<IContainer>(() =>
            {
                var containerBuilder = new ContainerBuilder();

                ConfigureEventHandling(containerBuilder);
                ConfigureCommandHandling(containerBuilder);
                containerBuilder.RegisterModule(new SqlStreamStoreModule());

                containerBuilder.UseAggregateSourceTesting(CreateExpectedFactComparer(), CreateExceptionComparer());

                containerBuilder.RegisterInstance(testOutputHelper);
                containerBuilder.RegisterType<XUnitLogger>().AsImplementedInterfaces();

                registerFunc?.Invoke(containerBuilder);

                return containerBuilder.Build();
            });
        }

        protected abstract void ConfigureCommandHandling(ContainerBuilder builder);
        protected abstract void ConfigureEventHandling(ContainerBuilder builder);

        protected virtual IExpectedFactComparer CreateExpectedFactComparer()
        {
            var comparer = new CompareLogic();
            return new CompareNetObjectsBasedExpectedFactComparer(comparer);
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
