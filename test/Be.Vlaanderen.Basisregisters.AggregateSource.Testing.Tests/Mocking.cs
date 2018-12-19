namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    using Moq;

    /// <summary>
    /// Class that provides a small abstraction over Moq to reuse Mock setups
    /// </summary>
    /// <typeparam name="TMoq"></typeparam>
    /// <typeparam name="TMockingSetup"></typeparam>
    public class Mocking<TMoq, TMockingSetup>
        where TMoq : class
        where TMockingSetup : MockingSetup<TMoq>, new()
    {
        private readonly Mock<TMoq> _moq;
        private readonly TMockingSetup _setup;

        public Mocking()
        {
            _moq = new Mock<TMoq>();
            _setup = new TMockingSetup {Moq = _moq};
        }

        public TMoq Object => _moq.Object;

        public TMockingSetup When()
        {
            return _setup;
        }
    }
}
