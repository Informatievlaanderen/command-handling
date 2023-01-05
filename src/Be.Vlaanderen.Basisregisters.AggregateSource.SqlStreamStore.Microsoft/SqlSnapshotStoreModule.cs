namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Microsoft
{
    using System;
    using DependencyInjection;
    using global::Microsoft.Extensions.DependencyInjection;
    using SqlStreamStore;

    public class SqlSnapshotStoreModule : IServiceCollectionModule
    {
        private readonly string _snapshotConnectionString;
        private readonly string _schema;
        private readonly Action<MsSqlSnapshotStoreSettings> _settingsFunc;

        /// <summary>
        /// Register an in-memory SqlStreamStore
        /// </summary>
        public SqlSnapshotStoreModule() { }

        /// <summary>
        /// Register a SQL Server SqlStreamStore
        /// </summary>
        /// <param name="snapshotConnectionString"></param>
        /// <param name="schema"></param>
        public SqlSnapshotStoreModule(string snapshotConnectionString, string schema) :
            this(snapshotConnectionString, schema, null) { }

        /// <summary>
        /// Register a SQL Server SqlStreamStore
        /// </summary>
        /// <param name="snapshotConnectionString"></param>
        /// <param name="schema"></param>
        /// <param name="settingsFunc"></param>
        public SqlSnapshotStoreModule(
            string snapshotConnectionString,
            string schema,
            Action<MsSqlSnapshotStoreSettings> settingsFunc)
        {
            _snapshotConnectionString = snapshotConnectionString;
            _schema = schema;
            _settingsFunc = settingsFunc;
        }

        public void Load(IServiceCollection services)
        {
            services.ConfigureSqlStreamStore(_snapshotConnectionString, _schema, _settingsFunc);
        }
    }
}
