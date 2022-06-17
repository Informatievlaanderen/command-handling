namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting.InMemory
{
    using System;

    internal sealed class InMemorySnapshot
    {
        internal int Id { get; }
        internal string StreamId { get; }
        internal DateTime Created { get; }
        internal SnapshotContainer Snapshot { get; }

        internal InMemorySnapshot(
            int id,
            string streamId,
            DateTime created,
            SnapshotContainer snapshot)
        {
            Id = id;
            StreamId = streamId;
            Created = created;
            Snapshot = snapshot;
        }
    }
}
