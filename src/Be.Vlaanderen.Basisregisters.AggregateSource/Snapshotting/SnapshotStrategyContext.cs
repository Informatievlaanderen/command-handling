namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting
{
    using System;
    using System.Collections.Immutable;

    public class SnapshotStrategyContext
    {
        public Aggregate Aggregate { get; }
        public ImmutableList<EventWithMetadata> Events { get; }
        public int StreamVersion { get; }

        public SnapshotStrategyContext(
            Aggregate aggregate,
            ImmutableList<EventWithMetadata> events,
            int streamVersion)
        {
            Aggregate = aggregate ?? throw new ArgumentNullException(nameof(aggregate));
            Events = events ?? throw new ArgumentNullException(nameof(events));
            StreamVersion = streamVersion;
        }
    }
}
