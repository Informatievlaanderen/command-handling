namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System.Collections.Generic;

    public interface ICommandHandlerResolver
    {
        List<ReturnHandler<CommandMessage<TCommand>>> Resolve<TCommand>() where TCommand : class;
    }
}
