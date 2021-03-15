namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    public class SnapshotContainer
    {
        public SnapshotInfo Info { get; set; } = new();
 
        public string Data { get; set; }
    }

    public class SnapshotInfo
    {
        public int Position { get; set; }

        public string Type { get; set; }
    }
}