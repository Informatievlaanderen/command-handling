namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System;
    using System.Collections.Generic;

    public class CommandHandlerResolver : ICommandHandlerResolver
    {
        private readonly HashSet<Type> _knownCommandTypes = new HashSet<Type>();
        private readonly Dictionary<Type, object> _handlers = new Dictionary<Type, object>();

        public CommandHandlerResolver(params CommandHandlerModule[] commandHandlerModules)
        {
            foreach(var module in commandHandlerModules)
            {
                foreach(var handlerRegistration in module.HandlerRegistrations)
                {
                    if (!_knownCommandTypes.Add(handlerRegistration.CommandType))
                        throw new InvalidOperationException(
                            "Attempt to register multiple handlers for command type {0}".FormatWith(handlerRegistration.CommandType));

                    _handlers[handlerRegistration.RegistrationType] = handlerRegistration.HandlerInstance;
                }
            }
        }

        public IEnumerable<Type> KnownCommandTypes => _knownCommandTypes;

        public ReturnHandler<CommandMessage<TCommand>>? Resolve<TCommand>() where TCommand : class
        {
            if(_handlers.TryGetValue(typeof(ReturnHandler<CommandMessage<TCommand>>), out var handler))
                return (ReturnHandler<CommandMessage<TCommand>>) handler;

            return null;
        }
    }
}
