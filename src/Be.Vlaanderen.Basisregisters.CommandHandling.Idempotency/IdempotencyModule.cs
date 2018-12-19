namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency
{
    using System;
    using Autofac;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class IdempotencyModule : Module
    {
        public IdempotencyModule(
            IServiceCollection services,
            string connectionString,
            IdempotencyMigrationsTableInfo migrationsTableInfo,
            IdempotencyTableInfo idempotencyTableInfo,
            ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<IdempotencyModule>();

            services.AddDbContext<IdempotencyContext>(
                options => options
                    .UseLoggerFactory(loggerFactory)
                    .UseSqlServer(
                        connectionString,
                        x => x.MigrationsHistoryTable(migrationsTableInfo.TableName, migrationsTableInfo.Schema)));

            services.AddSingleton(new IdempotencyConnectionInfo(connectionString));
            services.AddSingleton(migrationsTableInfo);
            services.AddSingleton(idempotencyTableInfo);

            logger.LogInformation(
                "Added {Context} to services:" +
                Environment.NewLine +
                "\tSchema: {Schema}" +
                Environment.NewLine +
                "\tTableName: {TableName}",
                nameof(IdempotencyContext), migrationsTableInfo.Schema, migrationsTableInfo.TableName);
        }
    }
}
