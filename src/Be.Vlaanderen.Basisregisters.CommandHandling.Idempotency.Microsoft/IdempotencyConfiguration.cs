namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Microsoft
{
    using System;
    using Be.Vlaanderen.Basisregisters.Converters.Timestamp;
    using Newtonsoft.Json;

    public class IdempotencyConfiguration
    {
        public const string Section = "Idempotency";

        [JsonConverter(typeof(TimestampConverter))]
        public DateTime Created => DateTime.Now;

        public string? ConnectionString { get; set; }

        public IdempotencyConfiguration Obfuscate()
        {
            return new IdempotencyConfiguration
            {
                ConnectionString = Obfuscator.ObfuscateConnectionString(ConnectionString),
            };
        }
    }
}
