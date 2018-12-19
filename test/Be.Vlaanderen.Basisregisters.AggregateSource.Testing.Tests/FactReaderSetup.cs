namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    using System.Threading.Tasks;
    using Moq;

    public class FactReaderSetup : MockingSetup<IFactReader>
    {
        public FactReaderSetup EventsExist(Fact[] events)
        {
            Moq.Setup(x => x.RetrieveFacts(It.IsAny<long>())).Returns(Task.FromResult(events));

            return this;
        }

        public FactReaderSetup NoEventsExist()
        {
            return EventsExist(new Fact[0]);
        }
    }
}