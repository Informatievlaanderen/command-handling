namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    using System.Threading;
    using System.Threading.Tasks;
    using Snapshotting;

    /// <summary>
    /// Represents the store where the snapshots are stored
    /// </summary>
    public interface ISnapshotStore
    {
        Task SaveSnapshotAsync(string identifier, SnapshotContainer snapshot, CancellationToken cancellationToken);

        Task<SnapshotContainer?> FindLatestSnapshotAsync(string identifier, CancellationToken cancellationToken);
    }
}
