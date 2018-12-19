namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    using System;
    using System.Threading.Tasks;
    using Moq;

    public class HandlerResolverSetup : MockingSetup<IHandlerResolver>
    {
        public HandlerResolverSetup ResolvesDummyHandler()
        {
            Moq.Setup(x => x.ResolveHandlerFor(It.IsAny<object>())).Returns(o => Task.FromResult(0L));

            return this;
        }

        public HandlerResolverSetup HandlerThrows(Exception exception)
        {
            Moq.Setup(x => x.ResolveHandlerFor(It.IsAny<object>())).Returns(o => throw exception);

            return this;
        }

        public HandlerResolverSetup ResolvesHandler(Func<object, Task<long>> handler)
        {
            Moq.Setup(x => x.ResolveHandlerFor(It.IsAny<object>())).Returns(handler);

            return this;
        }
    }
}
