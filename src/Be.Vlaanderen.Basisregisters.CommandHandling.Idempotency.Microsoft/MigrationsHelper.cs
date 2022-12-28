namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Microsoft
{
    using global::Microsoft.EntityFrameworkCore;
    using global::Microsoft.Extensions.Logging;

    public static class MigrationsHelper
    {
        internal static IdempotencyTableInfo TableInfo { get; private set; }

        public static void Run(
            string connectionString,
            string migrationsSchema,
            string migrationsTableName,
            IdempotencyTableInfo tableInfo,
            ILoggerFactory loggerFactory = null)
        {
            var migratorOptions = new DbContextOptionsBuilder<IdempotencyContext>()
                .UseSqlServer(
                    connectionString,
                    x => x.MigrationsHistoryTable(migrationsTableName, migrationsSchema));

            if (loggerFactory != null)
                migratorOptions = migratorOptions.UseLoggerFactory(loggerFactory);

            TableInfo = tableInfo;

            using (var migrator = new IdempotencyContext(migratorOptions.Options, tableInfo))
                migrator.Database.Migrate();
        }
    }
}
