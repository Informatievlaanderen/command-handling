namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Tests
{
    using System;

    public class Model
    {
        public Model()
        {
            KnownIdentifier = "aggregate/" + Guid.NewGuid();
            UnknownIdentifier = "aggregate/" + Guid.NewGuid();
        }

        public string KnownIdentifier { get; }
        public string UnknownIdentifier { get; }
    }
}
