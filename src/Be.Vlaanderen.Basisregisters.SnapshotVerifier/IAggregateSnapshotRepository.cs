namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AggregateSource;

    public interface IAggregateSnapshotRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRootEntity, ISnapshotable
    {
        Task<IReadOnlyList<SnapshotIdentifier>> GetSnapshotsSinceId(
            int? snapshotId,
            CancellationToken cancellationToken);

        Task<AggregateWithVersion<TAggregateRoot>?> GetAggregateBySnapshot(int idToVerify);
    }
}
