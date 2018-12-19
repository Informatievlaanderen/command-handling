namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    public static class UseIdempotencyDatabaseMigrationsExtension
    {
        public static IApplicationBuilder UseIdempotencyDatabaseMigrations(this IApplicationBuilder app)
        {
            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();
            var connectionInfo = app.ApplicationServices.GetService<IdempotencyConnectionInfo>();
            var migrationsInfo = app.ApplicationServices.GetService<IdempotencyMigrationsTableInfo>();
            var tableInfo = app.ApplicationServices.GetService<IdempotencyTableInfo>();

            MigrationsHelper.Run(
                connectionInfo.ConnectionString,
                migrationsInfo.Schema,
                migrationsInfo.TableName,
                tableInfo,
                loggerFactory);

            return app;
        }
    }
}
