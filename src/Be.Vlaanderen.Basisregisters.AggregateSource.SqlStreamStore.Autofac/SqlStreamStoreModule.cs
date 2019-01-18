namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Autofac
{
    using System;
    using AggregateSource;
    using SqlStreamStore;
    using global::Autofac;
    using global::SqlStreamStore;

    public class SqlStreamStoreModule : Module
    {
        private readonly string _eventsConnectionString;
        private readonly string _schema;
        private readonly Action<MsSqlStreamStoreSettings> _settingsFunc;

        /// <summary>
        /// Register an in-memory SqlStreamStore
        /// </summary>
        public SqlStreamStoreModule() { }

        /// <summary>
        /// Register a SQL Server SqlStreamStore
        /// </summary>
        /// <param name="eventsConnectionString"></param>
        /// <param name="schema"></param>
        public SqlStreamStoreModule(string eventsConnectionString, string schema) :
            this(eventsConnectionString, schema, null) { }

        /// <summary>
        /// Register a SQL Server SqlStreamStore
        /// </summary>
        /// <param name="eventsConnectionString"></param>
        /// <param name="schema"></param>
        /// <param name="settingsFunc"></param>
        public SqlStreamStoreModule(
            string eventsConnectionString,
            string schema,
            Action<MsSqlStreamStoreSettings> settingsFunc)
        {
            _eventsConnectionString = eventsConnectionString;
            _schema = schema;
            _settingsFunc = settingsFunc;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterGeneric(typeof(Repository<>))
                .As(typeof(IAsyncRepository<>))
                .InstancePerLifetimeScope();

            if (string.IsNullOrWhiteSpace(_eventsConnectionString))
            {
                builder.RegisterType<InMemoryStreamStore>()
                    .As<InMemoryStreamStore>()
                    .As<IStreamStore>()
                    .SingleInstance();
            }
            else
            {
                var settings = new MsSqlStreamStoreSettings(_eventsConnectionString) { Schema = _schema };

                _settingsFunc?.Invoke(settings);

                builder.RegisterInstance(settings);
                builder.RegisterType<MsSqlStreamStore>()
                    .As<MsSqlStreamStore>()
                    .As<IStreamStore>()
                    .As<IReadonlyStreamStore>()
                    .SingleInstance();
            }
        }
    }
}
