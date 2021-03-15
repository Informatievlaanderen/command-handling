namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Tests.Framework
{
    public class SnapshotStub
    {
        public int Value { get; set; }

        public SnapshotStub() { }

        public SnapshotStub(int value) => Value = value;

        public override bool Equals(object obj) => Equals(obj as SnapshotStub);

        private bool Equals(SnapshotStub snapshot) => !ReferenceEquals(snapshot, null) && Value.Equals(snapshot.Value);

        public override int GetHashCode()
        {
            unchecked
            {
                return Value.GetHashCode() * 10 + 2;
            }
        }
    }
}
