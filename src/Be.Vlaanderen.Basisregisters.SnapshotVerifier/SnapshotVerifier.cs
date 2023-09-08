namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AggregateSource;
    using KellermanSoftware.CompareNetObjects;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class SnapshotVerifier<TAggregateRoot, TStreamId> : BackgroundService
        where TAggregateRoot : class, IAggregateRootEntity, ISnapshotable
        where TStreamId : class
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly Func<TAggregateRoot, TStreamId> _streamIdFactory;
        private readonly List<string> _membersToIgnore;
        private readonly Dictionary<Type, IEnumerable<string>> _collectionMatchingSpec;
        private readonly ISnapshotVerificationNotifier? _snapshotVerificationNotifier;
        private readonly ISnapshotVerificationRepository _snapshotVerificationRepository;
        private readonly IAggregateSnapshotRepository<TAggregateRoot> _aggregateSnapshotRepository;
        private readonly IAggregateEventsRepository<TAggregateRoot, TStreamId> _aggregateEventsRepository;
        private readonly ILogger<SnapshotVerifier<TAggregateRoot, TStreamId>> _logger;

        /// <summary>
        /// SnapshotVerifier.
        /// </summary>
        /// <param name="applicationLifetime"></param>
        /// <param name="streamIdFactory"></param>
        /// <param name="membersToIgnore"></param>
        /// <param name="collectionMatchingSpec">
        /// Sometimes one wants to match items between collections by some key first, and then compare the matched objects.
        /// Without this, the comparer basically says there is no match in collection B for any given item in collection A that doesn't Compare with a result of true.
        /// </param>
        /// <param name="snapshotVerificationRepository"></param>
        /// <param name="aggregateSnapshotRepository"></param>
        /// <param name="aggregateEventsRepository"></param>
        /// <param name="snapshotVerificationNotifier"></param>
        /// <param name="loggerFactory"></param>
        public SnapshotVerifier(
            IHostApplicationLifetime applicationLifetime,
            Func<TAggregateRoot, TStreamId> streamIdFactory,
            List<string> membersToIgnore,
            Dictionary<Type, IEnumerable<string>> collectionMatchingSpec,
            ISnapshotVerificationRepository snapshotVerificationRepository,
            IAggregateSnapshotRepository<TAggregateRoot> aggregateSnapshotRepository,
            IAggregateEventsRepository<TAggregateRoot, TStreamId> aggregateEventsRepository,
            ISnapshotVerificationNotifier? snapshotVerificationNotifier,
            ILoggerFactory loggerFactory)
        {
            _applicationLifetime = applicationLifetime;
            _streamIdFactory = streamIdFactory;
            _membersToIgnore = membersToIgnore;
            _collectionMatchingSpec = collectionMatchingSpec;
            _snapshotVerificationRepository = snapshotVerificationRepository;
            _aggregateSnapshotRepository = aggregateSnapshotRepository;
            _aggregateEventsRepository = aggregateEventsRepository;
            _snapshotVerificationNotifier = snapshotVerificationNotifier;
            _logger = loggerFactory.CreateLogger<SnapshotVerifier<TAggregateRoot, TStreamId>>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting snapshot verifier");

            var lastProcessedSnapshotId = await _snapshotVerificationRepository.MaxSnapshotId(stoppingToken);
            var idsToVerify = await _aggregateSnapshotRepository.GetSnapshotsSinceId(lastProcessedSnapshotId);

            if (!idsToVerify.Any())
            {
                _applicationLifetime.StopApplication();
                return;
            }

            foreach (var idToVerify in idsToVerify.OrderBy(x => x.SnapshotId))
            {
                _logger.LogInformation("Verifying snapshot for {SnapshotId}", idToVerify.SnapshotId);

                var aggregateBySnapshot =
                    await _aggregateSnapshotRepository.GetAggregateBySnapshot(idToVerify.SnapshotId);
                if (aggregateBySnapshot is null)
                {
                    _logger.LogCritical("Could not retrieve snapshot blob for snapshot id {SnapshotId}",
                        idToVerify.SnapshotId);
                    continue;
                }

                var aggregateByEvents = await _aggregateEventsRepository.GetAggregateByEvents(
                    _streamIdFactory(aggregateBySnapshot.Aggregate),
                    (int)aggregateBySnapshot.StreamVersion,
                    stoppingToken);
                if (aggregateByEvents is null)
                {
                    _logger.LogCritical("Could not retrieve stream from stream store for {StreamId}",
                        idToVerify.StreamId);
                    continue;
                }

                var compareLogic = new CompareLogic(new ComparisonConfig
                {
                    MembersToIgnore = new List<string> { "_recorder", "_router", "_applier", "_lastEvent", "Strategy" }
                        .Concat(_membersToIgnore).ToList(),
                    ComparePrivateFields = true,
                    CompareBackingFields = false, // ONLY ignores compiler-generated backing fields.
                    ComparePrivateProperties = true,
                    IgnoreCollectionOrder = true,
                    CollectionMatchingSpec = _collectionMatchingSpec
                });

                var verificationState = new SnapshotVerificationState(idToVerify.SnapshotId);

                var comparisonResult = compareLogic.Compare(aggregateBySnapshot.Aggregate, aggregateByEvents);
                if (!comparisonResult.AreEqual)
                {
                    _logger.LogCritical("Snapshot {SnapshotId} does not match aggregate from events",
                        idToVerify.SnapshotId);
                    verificationState.Status = SnapshotStateStatus.Failed;
                    verificationState.Differences = comparisonResult.DifferencesString;
                    _snapshotVerificationNotifier?.NotifyInvalidSnapshot(idToVerify.SnapshotId, comparisonResult.DifferencesString);
                }
                else
                {
                    verificationState.Status = SnapshotStateStatus.Verified;
                }

                await _snapshotVerificationRepository.AddVerificationState(verificationState, stoppingToken);
            }

            _logger.LogInformation("Stopping snapshot verifier");

            _applicationLifetime.StopApplication();
        }
    }
}
