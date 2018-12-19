namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System;
    using System.Collections.Generic;

    public static class CommandMessageExtensions
    {
        public static void AddMetadata(this CommandMessage commandMessage, string key, object value)
        {
            if (commandMessage.Metadata.ContainsKey(key))
                throw new ArgumentException($"Element with key '{key}' already exists.");

            commandMessage.Metadata.Add(key, value);
        }

        public static void AddMetadata(this CommandMessage commandMessage, IDictionary<string, object> metadataDictionary)
        {
            foreach (var keyValuePair in metadataDictionary)
                AddMetadata(commandMessage, keyValuePair.Key, keyValuePair.Value);
        }
    }
}
