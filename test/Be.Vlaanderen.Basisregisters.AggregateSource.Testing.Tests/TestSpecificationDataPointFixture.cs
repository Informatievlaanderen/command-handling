namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    using System;
    using NUnit.Framework;

    public abstract class TestSpecificationDataPointFixture
    {
        [Datapoint]
        public static ExpectedFact[] NoEvents = new ExpectedFact[0];

        [Datapoint]
        public static ExpectedFact[] OneEvent = new[] { new ExpectedFact("Aggregate/" + Guid.NewGuid(), new object()) };

        [Datapoint]
        public static ExpectedFact[] TwoEventsOfDifferentSources = new[]
        {
            new ExpectedFact("Aggregate/" + Guid.NewGuid(), new object()),
            new ExpectedFact("Aggregate/" + Guid.NewGuid(), new object())
        };

        [Datapoint]
        public static ExpectedFact[] TwoEventsOfTheSameSource = new[]
        {
            new ExpectedFact("Aggregate/" + new Guid("C8F75337-62BA-41F0-B57B-10171388FD6F"), new object()),
            new ExpectedFact("Aggregate/" + new Guid("C8F75337-62BA-41F0-B57B-10171388FD6F"), new object())
        };

        [Datapoint] public static object Message = new object();

        [Datapoint] public static Exception Exception = new Exception("Message");
    }
}