namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    using System.Threading;
    using System.Threading.Tasks;
    using AggregateSource;

    public interface IAggregateEventsRepository<TAggregateRoot, TStreamId>
        where TAggregateRoot : class, IAggregateRootEntity, ISnapshotable
        where TStreamId : class
    {
        Task<TAggregateRoot?> GetAggregateByEvents(
            TStreamId streamId,
            int snapshotAggregateStreamVersion,
            CancellationToken stoppingToken);
    }
}
