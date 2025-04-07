namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System;
    using System.Collections.Generic;

    public interface ICommandHandlerResolver
    {
        ReturnHandler<CommandMessage<TCommand>>? Resolve<TCommand>() where TCommand : class;

        IEnumerable<Type> KnownCommandTypes { get; }
    }
}
