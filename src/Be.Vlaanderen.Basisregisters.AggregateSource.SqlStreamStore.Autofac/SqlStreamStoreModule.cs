namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Autofac
{
    using AggregateSource;
    using SqlStreamStore;
    using global::Autofac;
    using global::SqlStreamStore;

    public class SqlStreamStoreModule : Module
    {
        private readonly string _eventsConnectionString;
        private readonly string _schema;

        /// <summary>
        /// Register an in-memory SqlStreamStore
        /// </summary>
        public SqlStreamStoreModule() { }

        /// <summary>
        /// Register a SQL Server SqlStreamStore
        /// </summary>
        /// <param name="eventsConnectionString"></param>
        /// <param name="schema"></param>
        public SqlStreamStoreModule(string eventsConnectionString, string schema)
        {
            _eventsConnectionString = eventsConnectionString;
            _schema = schema;
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
                builder.RegisterInstance(new MsSqlStreamStoreSettings(_eventsConnectionString) { Schema = _schema });
                builder.RegisterType<MsSqlStreamStore>()
                    .As<MsSqlStreamStore>()
                    .As<IStreamStore>()
                    .SingleInstance();
            }
        }
    }
}
