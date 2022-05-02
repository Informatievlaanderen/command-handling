namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting
{
    using System;

    public class IntervalStrategy : ISnapshotStrategy
    {
        private readonly int _interval;

        private IntervalStrategy(int interval) => _interval = interval;

        public static IntervalStrategy SnapshotEvery(int interval) => new IntervalStrategy(interval);
        public static IntervalStrategy Default => SnapshotEvery(1000);

        public bool ShouldCreateSnapshot(SnapshotStrategyContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return ShouldCreateSnapshot(
                context.StreamVersion - context.Events.Count,
                context.StreamVersion,
                _interval);
        }

        private static bool ShouldCreateSnapshot(
            long startPosition,
            long endPosition,
            int snapshotInterval)
        {
            for (var i = startPosition; i < endPosition; i++)
            {
                if (i > 0 && i % snapshotInterval == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
