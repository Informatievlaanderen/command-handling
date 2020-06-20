namespace Be.Vlaanderen.Basisregisters.CommandHandling.Tests
{
    using System;
    using System.Collections.Generic;
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

        private class TestICommandHandlerModuleForPipeOrder : CommandHandlerModule
        {
            public List<string> ExecutionOrder = new List<string>();

            public TestICommandHandlerModuleForPipeOrder()
            {
                For<Command>().Pipe(next => async (m, c) =>
                {
                    ExecutionOrder.Add("Pipe.Before");
                    var result = await next(m, c);
                    ExecutionOrder.Add("Pipe.After");
                    return result;
                }).Handle((message, ct) =>
                {
                    ExecutionOrder.Add("Handle");
                    return Task.FromResult(0L);
                });
            }
        }

        private class TestICommandHandlerModuleForMultiplePipes : CommandHandlerModule
        {
            public List<string> ExecutionOrder = new List<string>();

            public TestICommandHandlerModuleForMultiplePipes()
            {
                For<Command>()
                    .Pipe(next => async (m, c) =>
                    {
                        ExecutionOrder.Add("Pipe1.Before");
                        var result = await next(m, c);
                        ExecutionOrder.Add("Pipe1.After");
                        return result;
                    })
                    .Pipe(next => async (m, c) =>
                    {
                        ExecutionOrder.Add("Pipe2.Before");
                        var result = await next(m, c);
                        ExecutionOrder.Add("Pipe2.After");
                        return result;
                    })
                    .Handle((message, ct) =>
                    {
                        ExecutionOrder.Add("Handle");
                        return Task.FromResult(0L);
                    });
            }
        }

        private class TestICommandHandlerModuleForMultiplePipesPassingValues : CommandHandlerModule
        {
            public List<string> ExecutionOrder = new List<string>();

            public TestICommandHandlerModuleForMultiplePipesPassingValues(long dummyValue)
            {
                For<Command>()
                    .Pipe(next => async (m, c) =>
                    {
                        ExecutionOrder.Add("Pipe1.Before");
                        var result = await next(m, c);
                        ExecutionOrder.Add($"Pipe1.After {result}");
                        return result;
                    })
                    .Pipe(next => async (m, c) =>
                    {
                        ExecutionOrder.Add("Pipe2.Before");
                        var result = await next(m, c);
                        ExecutionOrder.Add($"Pipe2.After {result}");
                        return dummyValue; // This pretends to return the position from sql stream store
                    })
                    .Handle((message, ct) =>
                    {
                        ExecutionOrder.Add("Handle");
                        return Task.FromResult(0);
                    });
            }
        }

        private sealed class TestCommandHandlerModule : TestICommandHandlerModule
        {
            public int CommandCounter;

            public TestCommandHandlerModule()
            {
                For<Command>()
                    .Handle((_, __) => Task.FromResult((long) CommandCounter++));
            }
        }

        private sealed class TestCommandHandlerModule2 : TestICommandHandlerModule
        {
            public int CommandCounter;

            public TestCommandHandlerModule2()
            {
                For<Command>()
                    .Handle((_, __) => Task.FromResult((long)CommandCounter++));
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
        public async Task PipesRunInCorrectOrder()
        {
            var module = new TestICommandHandlerModuleForPipeOrder();
            var resolver = new CommandHandlerResolver(module);

            await resolver.Dispatch(Guid.NewGuid(), new Command());

            module.ExecutionOrder.ShouldBe(new List<string>{"Pipe.Before", "Handle", "Pipe.After" });
        }

        [Fact]
        public void Throws_nice_exception_if_no_handler_found()
        {
            var module = new BadlyConfiguredCommandHandlerModule();
            var resolver = new CommandHandlerResolver(module);

            Func<Task<long>> dispatch = async ()=>await resolver.Dispatch(Guid.NewGuid(), new Command());

            dispatch.ShouldThrowAsync<ApplicationException>($"No handler was found for command {typeof(Command).FullName}");
        }

        [Fact]
        public async Task MultiplePipesRunInCorrectOrder()
        {
            var module = new TestICommandHandlerModuleForMultiplePipes();
            var resolver = new CommandHandlerResolver(module);

            await resolver.Dispatch(Guid.NewGuid(), new Command());

            module.ExecutionOrder.ShouldBe(new List<string> { "Pipe1.Before", "Pipe2.Before", "Handle", "Pipe2.After", "Pipe1.After" });
        }

        [Fact]
        public async Task MultiplePipesPassValueCorrectly()
        {
            var module = new TestICommandHandlerModuleForMultiplePipesPassingValues(5);
            var resolver = new CommandHandlerResolver(module);

            await resolver.Dispatch(Guid.NewGuid(), new Command());

            module.ExecutionOrder.ShouldBe(new List<string> { "Pipe1.Before", "Pipe2.Before", "Handle", "Pipe2.After -1", "Pipe1.After 5" });
        }

        [Fact]
        public void When_add_duplicate_command_then_should_throw()
        {
            var module1 = new TestCommandHandlerModule();
            var module2 = new TestCommandHandlerModule2();

            Action act = () => new CommandHandlerResolver(module1, module2);

            act.ShouldThrow<InvalidOperationException>();
        }
    }
}
