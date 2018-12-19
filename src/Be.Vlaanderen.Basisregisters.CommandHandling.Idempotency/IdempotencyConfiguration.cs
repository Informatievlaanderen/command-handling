namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency
{
    using System;
    using Converters.Timestamp;
    using Newtonsoft.Json;

    public class IdempotencyConfiguration
    {
        public static string Section = "Idempotency";

        [JsonConverter(typeof(TimestampConverter))]
        public DateTime Created => DateTime.Now;

        public string ConnectionString { get; set; }

        public IdempotencyConfiguration Obfuscate()
        {
            return new IdempotencyConfiguration
            {
                ConnectionString = Obfuscator.ObfuscateConnectionString(ConnectionString),
            };
        }
    }
}
