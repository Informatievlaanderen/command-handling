namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting
{
    using System;
    using System.Linq;

    public class AfterEventTypeStrategy : ISnapshotStrategy
    {
        private readonly Type[] _eventTypes;

        public AfterEventTypeStrategy(params Type[] eventTypes)
        {
            _eventTypes = eventTypes;
        }

        public bool ShouldCreateSnapshot(SnapshotStrategyContext context) => _eventTypes.Contains(context.Events.Last().Event.GetType());
    }
}
