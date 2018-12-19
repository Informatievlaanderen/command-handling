namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Threading.Tasks;

    public interface IHandlerResolver
    {
        Func<object, Task<long>> ResolveHandlerFor(object command);
    }
}
