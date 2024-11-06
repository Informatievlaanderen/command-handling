namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Snapshot strategy that returns true if all the given strategies returns true.
    /// </summary>
    public sealed class AllSnapshotStrategy : ISnapshotStrategy
    {
        private readonly IEnumerable<ISnapshotStrategy> _strategies;

        public AllSnapshotStrategy(IEnumerable<ISnapshotStrategy> strategies)
        {
            _strategies = strategies;
        }

        public bool ShouldCreateSnapshot(SnapshotStrategyContext context)
            => _strategies.All(x => x.ShouldCreateSnapshot(context));
    }
}
