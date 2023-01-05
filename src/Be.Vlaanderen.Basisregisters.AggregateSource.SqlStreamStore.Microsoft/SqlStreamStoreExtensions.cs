namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Microsoft
{
    using System;
    using global::Microsoft.Extensions.DependencyInjection;
    using Snapshotting;
    using Snapshotting.InMemory;

    public static class SqlStreamStoreExtensions
    {
        public static IServiceCollection ConfigureSqlStreamStore(
            this IServiceCollection serviceCollection,
            string snapshotConnectionString,
            string schema,
            Action<MsSqlSnapshotStoreSettings> settingsFunc)
        {
            serviceCollection.AddScoped(typeof(IAsyncRepository<>), typeof(Repository<>));

            if (string.IsNullOrWhiteSpace(snapshotConnectionString))
            {
                serviceCollection.AddSingleton<InMemorySnapshotStore, InMemorySnapshotStore>();
                serviceCollection.AddSingleton<ISnapshotStore, InMemorySnapshotStore>();
            }
            else
            {
                var settings = new MsSqlSnapshotStoreSettings(snapshotConnectionString) { Schema = schema };

                settingsFunc?.Invoke(settings);

                serviceCollection.AddTransient(_ => settings);
                serviceCollection.AddSingleton<MsSqlSnapshotStore, MsSqlSnapshotStore>();
                serviceCollection.AddSingleton<ISnapshotStore, MsSqlSnapshotStore>();
            }

            serviceCollection.AddTransient<Func<ISnapshotStore>>(c => c.GetRequiredService<ISnapshotStore>);

            return serviceCollection;
        }
    }
}
