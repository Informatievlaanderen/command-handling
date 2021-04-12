namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System.Threading.Tasks;

    public interface IFactReader
    {
        Task<Fact[]> RetrieveFacts(long fromPositionExclusive);
    }
}
