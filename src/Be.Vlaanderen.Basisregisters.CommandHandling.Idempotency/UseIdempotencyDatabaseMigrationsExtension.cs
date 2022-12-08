namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

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
