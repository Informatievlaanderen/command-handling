namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    using Moq;

    public class MockingSetup<TMoq> where TMoq : class
    {
        protected internal Mock<TMoq> Moq { get; set; }
    }
}