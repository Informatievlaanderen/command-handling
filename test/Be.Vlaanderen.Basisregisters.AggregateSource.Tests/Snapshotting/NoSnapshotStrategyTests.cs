namespace Be.Vlaanderen.Basisregisters.AggregateSource.Tests.Snapshotting
{
    using System.Collections.Immutable;
    using AggregateSource.Snapshotting;
    using Xunit;

    public class NoSnapshotStrategyTests
    {
        [Fact]
        public void NeverTakeSnapshot()
        {
            var strategy = (ISnapshotStrategy)new NoSnapshotStrategy();
            var ctx = new SnapshotStrategyContext(new Aggregate("1", 1, new AggregateRootEntityStub()), ImmutableList<EventWithMetadata>.Empty, 1);
            var result = strategy.ShouldCreateSnapshot(ctx);

            Assert.False(result);
        }
    }
}
