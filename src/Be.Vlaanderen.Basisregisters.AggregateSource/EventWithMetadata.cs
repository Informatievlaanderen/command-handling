namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    using System.Collections.Generic;

    public class EventWithMetadata
    {
        public object Event { get; }

        public IDictionary<string, object> Metadata { get;}

        public EventWithMetadata(object @event) : this(@event, null) { }

        public EventWithMetadata(object @event, IDictionary<string, object> metadata)
        {
            Event = @event;
            Metadata = metadata;
        }
    }
}
