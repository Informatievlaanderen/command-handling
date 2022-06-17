namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the store where the snapshots are stored
    /// </summary>
    public interface ISnapshotStore
    {
        /// <summary>
        /// Save a snapshot
        /// </summary>
        /// <param name="identifier">the stream identifier</param>
        /// <param name="snapshot">the snapshot container</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SaveSnapshotAsync(string identifier, SnapshotContainer snapshot, CancellationToken cancellationToken);

        /// <summary>
        /// Find the latest snapshot, returns null if none can be found.
        /// </summary>
        /// <param name="identifier">the stream identifier</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SnapshotContainer?> FindLatestSnapshotAsync(string identifier, CancellationToken cancellationToken);
    }
}
