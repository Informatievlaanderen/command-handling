namespace Be.Vlaanderen.Basisregisters.CommandHandling.SqlStreamStore.Autofac
{
    using System;
    using AggregateSource;
    using CommandHandling;
    using EventHandling;
    using SqlStreamStore;
    using global::Autofac;
    using global::SqlStreamStore;

    public static class ContainerBuilderExtensions
    {
        public static void RegisterSqlStreamStoreCommandHandler<T>(
            this ContainerBuilder containerBuilder,
            Func<IComponentContext, Func<ReturnHandler<CommandMessage>, T>> decorate)
            where T : CommandHandlerModule
        {
            var typeName = typeof(T).FullName;
            containerBuilder.RegisterType<T>()
                .Named<CommandHandlerModule>(typeName);

            containerBuilder.RegisterDecorator<CommandHandlerModule>(
                    (c, inner) =>
                        new StreamStoreCommandHandlerModule<T>(
                            c.Resolve<Func<IStreamStore>>(),
                            c.Resolve<Func<ConcurrentUnitOfWork>>(),
                            decorate(c),
                            c.Resolve<EventMapping>(),
                            c.Resolve<EventSerializer>()),
                    typeName)
                .As<CommandHandlerModule>();
        }

        [Obsolete("Use RegisterSqlStreamStoreCommandHandler which assumes EventMapping and EventSerializer to be registered in the container.", true)]
        public static void RegisterSqlStreamStoreCommandHandler<T>(
            this ContainerBuilder containerBuilder,
            Func<IComponentContext, Func<ReturnHandler<CommandMessage>, T>> decorate,
            EventMapping eventMapping,
            EventSerializer eventSerializer)
            where T : CommandHandlerModule
        {
            var typeName = typeof(T).FullName;
            containerBuilder.RegisterType<T>()
                .Named<CommandHandlerModule>(typeName);

            containerBuilder.RegisterDecorator<CommandHandlerModule>(
                    (c, inner) =>
                        new StreamStoreCommandHandlerModule<T>(
                            c.Resolve<Func<IStreamStore>>(),
                            c.Resolve<Func<ConcurrentUnitOfWork>>(),
                            decorate(c),
                            eventMapping,
                            eventSerializer),
                    typeName)
                .As<CommandHandlerModule>();
        }
    }
}
