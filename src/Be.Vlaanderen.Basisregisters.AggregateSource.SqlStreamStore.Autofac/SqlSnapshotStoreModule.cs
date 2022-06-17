namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Autofac
{
    using System;
    using AggregateSource;
    using global::Autofac;
    using Snapshotting;
    using Snapshotting.InMemory;
    using SqlStreamStore;

    public class SqlSnapshotStoreModule : Module
    {
        private readonly string _eventsConnectionString;
        private readonly string _schema;
        private readonly Action<MsSqlSnapshotStoreSettings> _settingsFunc;

        /// <summary>
        /// Register an in-memory SqlStreamStore
        /// </summary>
        public SqlSnapshotStoreModule() { }

        /// <summary>
        /// Register a SQL Server SqlStreamStore
        /// </summary>
        /// <param name="eventsConnectionString"></param>
        /// <param name="schema"></param>
        public SqlSnapshotStoreModule(string eventsConnectionString, string schema) :
            this(eventsConnectionString, schema, null) { }

        /// <summary>
        /// Register a SQL Server SqlStreamStore
        /// </summary>
        /// <param name="eventsConnectionString"></param>
        /// <param name="schema"></param>
        /// <param name="settingsFunc"></param>
        public SqlSnapshotStoreModule(
            string eventsConnectionString,
            string schema,
            Action<MsSqlSnapshotStoreSettings> settingsFunc)
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
                builder.RegisterType<InMemorySnapshotStore>()
                    .As<InMemorySnapshotStore>()
                    .As<ISnapshotStore>()
                    .SingleInstance();
            }
            else
            {
                var settings = new MsSqlSnapshotStoreSettings(_eventsConnectionString) { Schema = _schema };

                _settingsFunc?.Invoke(settings);

                builder.RegisterInstance(settings);
                builder.RegisterType<MsSqlSnapshotStore>()
                    .As<MsSqlSnapshotStore>()
                    .As<ISnapshotStore>()
                    .SingleInstance();
            }
        }
    }
}
