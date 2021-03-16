namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting
{
    public interface ISnapshotStrategy
    {
        /// <summary>
        /// Determines if a snapshot should be created.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool ShouldCreateSnapshot(SnapshotStrategyContext context);
    }
}
