namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System.Threading.Tasks;

    public interface IFactWriter
    {
        Task<long> PersistFacts(Fact[] facts);
    }
}
