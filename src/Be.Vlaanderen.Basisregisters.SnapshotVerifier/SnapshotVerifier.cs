namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    using System;
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
        private readonly ComparisonConfig _comparisonConfig;
        private readonly ISnapshotVerificationNotifier? _snapshotVerificationNotifier;
        private readonly ISnapshotVerificationRepository _snapshotVerificationRepository;
        private readonly IAggregateSnapshotRepository<TAggregateRoot> _aggregateSnapshotRepository;
        private readonly IAggregateEventsRepository<TAggregateRoot, TStreamId> _aggregateEventsRepository;
        private readonly ILogger<SnapshotVerifier<TAggregateRoot, TStreamId>> _logger;

        public SnapshotVerifier(
            IHostApplicationLifetime applicationLifetime,
            Func<TAggregateRoot, TStreamId> streamIdFactory,
            ComparisonConfig comparisonConfig,
            ISnapshotVerificationRepository snapshotVerificationRepository,
            IAggregateSnapshotRepository<TAggregateRoot> aggregateSnapshotRepository,
            IAggregateEventsRepository<TAggregateRoot, TStreamId> aggregateEventsRepository,
            ISnapshotVerificationNotifier? snapshotVerificationNotifier,
            ILoggerFactory loggerFactory)
        {
            _applicationLifetime = applicationLifetime;
            _streamIdFactory = streamIdFactory;
            _comparisonConfig = comparisonConfig;
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

            var compareLogic = new CompareLogic(_comparisonConfig);

            foreach (var idToVerify in idsToVerify.OrderBy(x => x.SnapshotId))
            {
                _logger.LogInformation("Verifying snapshot for {SnapshotId}", idToVerify.SnapshotId);

                var aggregateBySnapshot =
                    await _aggregateSnapshotRepository.GetAggregateBySnapshot(idToVerify.SnapshotId);
                if (aggregateBySnapshot is null)
                {
                    _logger.LogCritical(
                        "Could not retrieve snapshot blob for snapshot id {SnapshotId}",
                        idToVerify.SnapshotId);
                    continue;
                }

                var aggregateByEvents = await _aggregateEventsRepository.GetAggregateByEvents(
                    _streamIdFactory(aggregateBySnapshot.Aggregate),
                    (int)aggregateBySnapshot.StreamVersion,
                    stoppingToken);
                if (aggregateByEvents is null)
                {
                    _logger.LogCritical(
                        "Could not retrieve stream from stream store for {StreamId}",
                        idToVerify.StreamId);
                    continue;
                }

                var verificationState = new SnapshotVerificationState(idToVerify.SnapshotId);

                var comparisonResult = compareLogic.Compare(aggregateBySnapshot.Aggregate, aggregateByEvents);
                if (!comparisonResult.AreEqual)
                {
                    _logger.LogCritical(
                        "Snapshot {SnapshotId} does not match aggregate from events",
                        idToVerify.SnapshotId);

                    verificationState.Status = SnapshotStateStatus.Failed;
                    verificationState.Differences = comparisonResult.DifferencesString;

                    _snapshotVerificationNotifier?.NotifyInvalidSnapshot(
                        idToVerify.SnapshotId,
                        comparisonResult.DifferencesString);
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
