namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting
{
    public class SnapshotInfo
    {
        public long StreamVersion { get; set; }

        public string Type { get; set; } = string.Empty;
    }
}
