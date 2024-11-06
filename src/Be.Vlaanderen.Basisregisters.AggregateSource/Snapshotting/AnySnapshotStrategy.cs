namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Snapshot strategy that returns true if any of the given strategies returns true.
    /// </summary>
    public sealed class AnySnapshotStrategy : ISnapshotStrategy
    {
        private readonly IEnumerable<ISnapshotStrategy> _strategies;

        public AnySnapshotStrategy(IEnumerable<ISnapshotStrategy> strategies)
        {
            _strategies = strategies;
        }

        public bool ShouldCreateSnapshot(SnapshotStrategyContext context)
            => _strategies.Any(x => x.ShouldCreateSnapshot(context));
    }
}
