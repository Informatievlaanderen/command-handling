namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    using System;
    using System.Threading.Tasks;
    using Moq;

    public class ExpectedFactReaderSetup : MockingSetup<IExpectedFactReader>
    {
        public ExpectedFactReaderSetup EventsExist(ExpectedFact[] events)
        {
            Moq.Setup(x => x.RetrieveFacts(It.IsAny<long>())).Returns(Task.FromResult(events));

            return this;
        }

        public ExpectedFactReaderSetup NoEventsExist()
        {
            return EventsExist(Array.Empty<ExpectedFact>());
        }
    }
}
