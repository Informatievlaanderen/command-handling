namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandHandlerResolver : ICommandHandlerResolver
    {
        private readonly Dictionary<Type, List<object>> _handlers = new Dictionary<Type, List<object>>();

        public CommandHandlerResolver(params CommandHandlerModule[] commandHandlerModules)
        {
            foreach(var module in commandHandlerModules)
            {
                foreach(var handlerRegistration in module.HandlerRegistrations)
                {
                    if (!_handlers.ContainsKey(handlerRegistration.RegistrationType))
                        _handlers[handlerRegistration.RegistrationType] = new List<object>();

                    _handlers[handlerRegistration.RegistrationType].Add(handlerRegistration.HandlerInstance);
                }
            }
        }

        public List<ReturnHandler<CommandMessage<TCommand>>> Resolve<TCommand>() where TCommand : class
        {
            if (_handlers.TryGetValue(typeof(ReturnHandler<CommandMessage<TCommand>>), out var handlers))
                return handlers.Cast<ReturnHandler<CommandMessage<TCommand>>>().ToList();

            return null;
        }
    }
}
