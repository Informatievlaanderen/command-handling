namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting
{
    public class NoSnapshotStrategy : ISnapshotStrategy
    {
        public static NoSnapshotStrategy Instance => new NoSnapshotStrategy();

        public bool ShouldCreateSnapshot(SnapshotStrategyContext context) => false;
    }
}
