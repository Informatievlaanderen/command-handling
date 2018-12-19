namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    using System;

    public class AggregateRootEntityStub : AggregateRootEntity
    {
        public static readonly Func<AggregateRootEntityStub> Factory = () => new AggregateRootEntityStub();
    }
}