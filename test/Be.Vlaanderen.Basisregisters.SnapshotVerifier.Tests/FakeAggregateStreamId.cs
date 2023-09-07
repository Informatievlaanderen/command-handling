namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier.Tests
{
    using System.Collections.Generic;
    using AggregateSource;

    public class FakeAggregateStreamId : ValueObject<FakeAggregateStreamId>
    {
        private readonly int _id;

        public FakeAggregateStreamId(int id)
        {
            _id = id;
        }

        protected override IEnumerable<object> Reflect()
        {
            yield return _id;
        }

        public override string ToString() => $"testaggregate-{_id}";
    }
}
