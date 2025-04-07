namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting
{
    public class SnapshotContainer
    {
        public SnapshotInfo Info { get; set; } = new SnapshotInfo();

        public string Data { get; set; } = string.Empty;
    }
}
