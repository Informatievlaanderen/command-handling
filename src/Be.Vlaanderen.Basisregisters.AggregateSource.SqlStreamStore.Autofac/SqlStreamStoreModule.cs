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
        private readonly Action<MsSqlStreamStoreSettings> _streamStoreSettingsFunc;
        private readonly Action<MsSqlSnapshotStoreSettings> _snapshotSettingsFunc;

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
            : this(eventsConnectionString, schema, settingsFunc, null) { }

        /// <summary>
        /// Register a SQL Server SqlStreamStore
        /// </summary>
        /// <param name="eventsConnectionString"></param>
        /// <param name="schema"></param>
        /// <param name="streamStoreSettingsFunc"></param>
        /// <param name="snapshotSettingsFunc"></param>
        public SqlStreamStoreModule(
            string eventsConnectionString,
            string schema,
            Action<MsSqlStreamStoreSettings> streamStoreSettingsFunc,
            Action<MsSqlSnapshotStoreSettings> snapshotSettingsFunc)
        {
            _eventsConnectionString = eventsConnectionString;
            _schema = schema;
            _streamStoreSettingsFunc = streamStoreSettingsFunc;
            _snapshotSettingsFunc = snapshotSettingsFunc;
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
                    .As<IReadonlyStreamStore>()
                    .SingleInstance();
            }
            else
            {
                var streamStoreSettings = new MsSqlStreamStoreSettings(_eventsConnectionString) { Schema = _schema };
                var snapshotStoreSettings = new MsSqlSnapshotStoreSettings(_eventsConnectionString) { Schema = _schema };

                _streamStoreSettingsFunc?.Invoke(streamStoreSettings);
                _snapshotSettingsFunc?.Invoke(snapshotStoreSettings);

                builder.RegisterInstance(streamStoreSettings);
                builder.RegisterType<MsSqlStreamStore>()
                    .As<MsSqlStreamStore>()
                    .As<IStreamStore>()
                    .As<IReadonlyStreamStore>()
                    .SingleInstance();

                builder.RegisterInstance(snapshotStoreSettings);
                builder.RegisterType<MsSqlSnapshotStore>()
                    .As<MsSqlSnapshotStore>()
                    .As<ISnapshotStore>()
                    .SingleInstance();
            }
        }
    }
}
