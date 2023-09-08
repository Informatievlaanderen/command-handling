namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier.Tests
{
    using AggregateSource;

    public class FakeEntity : Entity
    {
        public int Identifier { get; }
        public int Value;

        private FakeEntity() : base(_ => {})
        {
        }

        public FakeEntity(int value)
            : this()
        {
            Identifier = value;
            Value = value;
        }

        public FakeEntity(int identifier, int value)
            : this()
        {
            Identifier = identifier;
            Value = value;
        }
    }
}
