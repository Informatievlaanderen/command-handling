namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore
{
    using global::SqlStreamStore.Streams;

    internal class HydrateContainer
    {
        public int ReadFrom { get; set; } = StreamVersion.Start;

        public object? Snapshot { get; set; }
    }
}
