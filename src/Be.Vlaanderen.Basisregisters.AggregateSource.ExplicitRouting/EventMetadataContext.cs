namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class EventMetadataContext : IDisposable
    {
        internal static readonly AsyncLocal<IDictionary<string, object>> MetadataAsync = new AsyncLocal<IDictionary<string, object>>();

        public EventMetadataContext(IDictionary<string, object> metadata) =>
            MetadataAsync.Value = metadata.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);

        public static void Add(string key, object value) => MetadataAsync.Value[key] = value;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        { }
    }
}
