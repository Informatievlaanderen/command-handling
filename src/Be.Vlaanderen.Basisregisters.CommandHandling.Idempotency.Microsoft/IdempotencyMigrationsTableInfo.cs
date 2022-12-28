namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Microsoft
{
    using System;

    public class IdempotencyMigrationsTableInfo
    {
        public const string DefaultMigrationsTableName = "__EFMigrationsHistoryIdempotency";

        public string Schema { get; }
        public string TableName { get; }

        public IdempotencyMigrationsTableInfo(string schema, string tableName = DefaultMigrationsTableName)
        {
            Schema = string.IsNullOrWhiteSpace(schema) ? throw new ArgumentException(nameof(schema)) : schema;
            TableName = string.IsNullOrWhiteSpace(tableName) ? throw new ArgumentException(nameof(tableName)) : tableName;
        }
    }
}
