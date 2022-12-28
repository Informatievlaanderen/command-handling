namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Microsoft
{
    using System;

    public class IdempotencyTableInfo
    {
        public string Schema { get; }
        public string TableName { get; }

        public IdempotencyTableInfo(string schema, string tableName = "ProcessedCommands")
        {
            Schema = string.IsNullOrWhiteSpace(schema) ? throw new ArgumentException(nameof(schema)) : schema;
            TableName = string.IsNullOrWhiteSpace(tableName) ? throw new ArgumentException(nameof(tableName)) : tableName;
        }
    }
}
