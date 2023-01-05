namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Microsoft
{
    using System;
    using AggregateSource;
    using DependencyInjection;
    using global::Microsoft.Extensions.DependencyInjection;
    using global::SqlStreamStore;
    using SqlStreamStore;

    public class SqlStreamStoreModule : IServiceCollectionModule
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

        public void Load(IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(Repository<>));

            if (string.IsNullOrWhiteSpace(_eventsConnectionString))
            {
                services.AddSingleton<InMemoryStreamStore, InMemoryStreamStore>();
                services.AddSingleton<IStreamStore, InMemoryStreamStore>();
                services.AddSingleton<IReadonlyStreamStore, InMemoryStreamStore>();
            }
            else
            {
                var settings = new MsSqlStreamStoreSettings(_eventsConnectionString) { Schema = _schema };

                _settingsFunc?.Invoke(settings);

                services.AddTransient(_ => settings);
                services.AddSingleton<MsSqlStreamStore, MsSqlStreamStore>();
                services.AddSingleton<IStreamStore, MsSqlStreamStore>();
                services.AddSingleton<IReadonlyStreamStore, MsSqlStreamStore>();
            }

            services.AddTransient<Func<IStreamStore>>(c => c.GetRequiredService<IStreamStore>);
            services.AddTransient<Func<IReadonlyStreamStore>>(c => c.GetRequiredService<IReadonlyStreamStore>);
        }
    }
}
