namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System.Threading.Tasks;

    public interface IExpectedFactReader
    {
        Task<ExpectedFact[]> RetrieveFacts(long fromPositionExclusive);
    }
}
