namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting
{
    public class AfterEventsCountStrategy : ISnapshotStrategy
    {
        private readonly int _numberOfEvents;

        public AfterEventsCountStrategy(int numberOfEvents)
        {
            _numberOfEvents = numberOfEvents;
        }

        public bool ShouldCreateSnapshot(SnapshotStrategyContext context) => context.Events.Count >= _numberOfEvents;
    }
}
