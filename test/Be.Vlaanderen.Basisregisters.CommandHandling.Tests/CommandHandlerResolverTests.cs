namespace Be.Vlaanderen.Basisregisters.CommandHandling.Tests
{
    using System;
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;

    public class CommandHandlerResolverTests
    {
        private class BadlyConfiguredCommandHandlerModule:CommandHandlerModule{}

        private class TestICommandHandlerModule : CommandHandlerModule
        {
            public int BaseCommandCounter;

            public override ICommandHandlerBuilder<CommandMessage<TCommand>> For<TCommand>()
            {
                return base.For<TCommand>().Pipe(finalHandler => (m, c) =>
                {
                    BaseCommandCounter++;
                    return finalHandler(m, c);
                });
            }
        }

        private sealed class TestCommandHandlerModule : TestICommandHandlerModule
        {
            public int CommandCounter;

            public TestCommandHandlerModule()
            {
                For<Command>()
                    .Finally((_, __) => Task.FromResult((long) CommandCounter++));
            }
        }

        [Fact]
        public async Task Can_dispatch()
        {
            var module = new TestCommandHandlerModule();
            var resolver = new CommandHandlerResolver(module);

            await resolver.Dispatch(Guid.NewGuid(), new Command());

            module.BaseCommandCounter.ShouldBe(1);
            module.CommandCounter.ShouldBe(1);
        }

        [Fact]
        public void Can_resolve_handler()
        {
            var module = new TestCommandHandlerModule();
            var resolver = new CommandHandlerResolver(module);

            var handler = resolver.Resolve<Command>();

            handler.ShouldNotBeNull();
        }

        [Fact]
        public void Throws_nice_exception_if_no_handler_found()
        {
            var module = new BadlyConfiguredCommandHandlerModule();
            var resolver = new CommandHandlerResolver(module);

            Func<Task<long>> dispatch = async ()=>await resolver.Dispatch(Guid.NewGuid(), new Command());

            dispatch.ShouldThrowAsync<ApplicationException>($"No handler was found for command {typeof(Command).FullName}");
        }
    }
}
