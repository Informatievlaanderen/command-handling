namespace Be.Vlaanderen.Basisregisters.CommandHandling.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;

    public class CommandHandlerModuleTests
    {
        [Fact]
        public void When_add_duplicate_command_then_should_throw()
        {
            var module = new CommandHandlerModule();
            module.For<Command>().Finally((_, __) => Task.FromResult(0L));

            Action act = () => module.For<Command>().Finally((_, __) => Task.FromResult(0L));

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_get_command_types()
        {
            var module = new CommandHandlerModule();

            module.For<Command>().Finally((_,__) => Task.FromResult(-1L));

            var commandTypes = module.CommandTypes.ToList();

            commandTypes.Count.ShouldBe(1);
            commandTypes.Single().ShouldBe(typeof(Command));
        }
    }
}
