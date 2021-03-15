namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Tests.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class AggregateRootEntityStub : AggregateRootEntity
    {
        public static readonly Func<AggregateRootEntityStub> Factory = () => new AggregateRootEntityStub();

        private readonly List<object> _recordedEvents;

        public AggregateRootEntityStub()
        {
            _recordedEvents = new List<object>();

            Register<EventStub>(_ => _recordedEvents.Add(_));
            Register<SnapshotStub>(_ => _recordedEvents.Add(_));
        }

        public IList<object> RecordedEvents => new ReadOnlyCollection<object>(_recordedEvents);
    }
}
