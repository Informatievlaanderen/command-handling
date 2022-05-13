namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System.Threading.Tasks;

    public interface IExpectedFactWriter
    {
        Task<long> PersistFacts(ExpectedFact[] facts);
    }
}
