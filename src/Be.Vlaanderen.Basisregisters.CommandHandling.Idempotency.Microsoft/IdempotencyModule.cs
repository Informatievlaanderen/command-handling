namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Microsoft
{
    using System;
    using DependencyInjection;
    using global::Microsoft.EntityFrameworkCore;
    using global::Microsoft.Extensions.DependencyInjection;
    using global::Microsoft.Extensions.Logging;

    public class IdempotencyModule : IServiceCollectionModule
    {
        private readonly string _connectionString;
        private readonly IdempotencyMigrationsTableInfo _migrationsTableInfo;
        private readonly IdempotencyTableInfo _idempotencyTableInfo;
        private readonly ILoggerFactory _loggerFactory;

        public IdempotencyModule(
            string connectionString,
            IdempotencyMigrationsTableInfo migrationsTableInfo,
            IdempotencyTableInfo idempotencyTableInfo,
            ILoggerFactory loggerFactory)
        {
            _connectionString = connectionString;
            _migrationsTableInfo = migrationsTableInfo;
            _idempotencyTableInfo = idempotencyTableInfo;
            _loggerFactory = loggerFactory;
        }

        public void Load(IServiceCollection services)
        {
            var logger = _loggerFactory.CreateLogger<IdempotencyModule>();

            services.AddDbContext<IdempotencyContext>(
                options => options
                    .UseLoggerFactory(_loggerFactory)
                    .UseSqlServer(
                        _connectionString,
                        x => x.MigrationsHistoryTable(_migrationsTableInfo.TableName, _migrationsTableInfo.Schema)));

            services.AddSingleton(new IdempotencyConnectionInfo(_connectionString));
            services.AddSingleton(_migrationsTableInfo);
            services.AddSingleton(_idempotencyTableInfo);

            logger.LogInformation(
                "Added {Context} to services:" +
                Environment.NewLine +
                "\tSchema: {Schema}" +
                Environment.NewLine +
                "\tTableName: {TableName}",
                nameof(IdempotencyContext), _migrationsTableInfo.Schema, _migrationsTableInfo.TableName);
        }
    }
}
