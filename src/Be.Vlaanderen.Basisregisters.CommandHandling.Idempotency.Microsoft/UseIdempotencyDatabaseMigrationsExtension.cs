namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Microsoft
{
    using System;
    using global::Microsoft.AspNetCore.Builder;
    using global::Microsoft.Extensions.DependencyInjection;
    using global::Microsoft.Extensions.Logging;

    public static class UseIdempotencyDatabaseMigrationsExtension
    {
        public static IApplicationBuilder UseIdempotencyDatabaseMigrations(this IApplicationBuilder app)
        {
            app.ApplicationServices.MigrateIdempotencyDatabase();

            return app;
        }

        public static void MigrateIdempotencyDatabase(this IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            var connectionInfo = serviceProvider.GetService<IdempotencyConnectionInfo>();
            var migrationsInfo = serviceProvider.GetService<IdempotencyMigrationsTableInfo>();
            var tableInfo = serviceProvider.GetService<IdempotencyTableInfo>();

            MigrationsHelper.Run(
                connectionInfo.ConnectionString,
                migrationsInfo.Schema,
                migrationsInfo.TableName,
                tableInfo,
                loggerFactory);
        }
    }
}
