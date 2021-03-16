namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Tests.Framework
{
    using Snapshotting;

    public class AggregateRootEntitySnapshotableStub : AggregateRootEntityStub, ISnapshotable
    {
        public ISnapshotStrategy Strategy => IntervalStrategy.Default;
        public object TakeSnapshot() => new  object();
    }
}
