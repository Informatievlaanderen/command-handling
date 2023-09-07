namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AggregateSource;
    using AggregateSource.Snapshotting;
    using EventHandling;
    using Microsoft.Extensions.Logging;

    public class AggregateSnapshotRepository<TAggregateRoot> : IAggregateSnapshotRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRootEntity, ISnapshotable
    {
        private readonly MsSqlSnapshotStoreQueries _snapshotStoreQueries;

        private readonly EventDeserializer _eventDeserializer;
        private readonly EventMapping _eventMapping;
        private readonly Func<TAggregateRoot> _aggregateRootFactory;

        private readonly ILogger<AggregateSnapshotRepository<TAggregateRoot>> _logger;

        public AggregateSnapshotRepository(
            MsSqlSnapshotStoreQueries snapshotStoreQueries,
            EventDeserializer eventDeserializer,
            EventMapping eventMapping,
            Func<TAggregateRoot> aggregateRootFactory,
            ILoggerFactory loggerFactory)
        {
            _snapshotStoreQueries = snapshotStoreQueries;
            _eventDeserializer = eventDeserializer;
            _eventMapping = eventMapping;
            _aggregateRootFactory = aggregateRootFactory;
            _logger = loggerFactory.CreateLogger<AggregateSnapshotRepository<TAggregateRoot>>();
        }

        public async Task<IReadOnlyList<SnapshotIdentifier>> GetSnapshotsSinceId(
            int? snapshotId,
            CancellationToken cancellationToken)
        {
            if (!await _snapshotStoreQueries.DoesTableExist())
            {
                _logger.LogError("Snapshot table does not exist");
                return new List<SnapshotIdentifier>();
            }

            var idsToVerify = (await _snapshotStoreQueries.GetSnapshotIdsToVerify(snapshotId))?.ToList();
            if (idsToVerify is null || !idsToVerify.Any())
            {
                _logger.LogInformation("Could not retrieve snapshot ids to verify");
                return new List<SnapshotIdentifier>();
            }

            return idsToVerify;
        }

        public async Task<AggregateWithVersion<TAggregateRoot>?> GetAggregateBySnapshot(int idToVerify)
        {
            var snapshotBlob = await _snapshotStoreQueries.GetSnapshotBlob(idToVerify);
            if (snapshotBlob is null)
            {
                return null;
            }

            var snapshotContainer =
                (SnapshotContainer)_eventDeserializer.DeserializeObject(snapshotBlob, typeof(SnapshotContainer));
            var snapshotType = _eventMapping.GetEventType(snapshotContainer.Info.Type);
            var snapshot = _eventDeserializer.DeserializeObject(snapshotContainer.Data, snapshotType);

            var snapshotAggregate = _aggregateRootFactory.Invoke();
            snapshotAggregate.RestoreSnapshot(snapshot);
            return new AggregateWithVersion<TAggregateRoot>(snapshotAggregate, snapshotContainer.Info.StreamVersion);
        }

    }

    public record AggregateWithVersion<TAggregateRoot>(TAggregateRoot Aggregate, long StreamVersion)
        where TAggregateRoot : class, IAggregateRootEntity, ISnapshotable;
}
