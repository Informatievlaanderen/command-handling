namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public static class CommandHandlerResolverExtensions
    {
        private static readonly MethodInfo DispatchInternalMethod = typeof(CommandHandlerResolverExtensions)
            .GetRuntimeMethods()
            .Single(m => m.Name.Equals(nameof(DispatchInternal), StringComparison.Ordinal));

        public static async Task<long[]> Dispatch(
            this ICommandHandlerResolver handlerResolver,
            Guid commandId,
            object command,
            IDictionary<string, object> metadata = null,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            metadata ??= new Dictionary<string, object>();

            var eventType = command.GetType();
            var dispatchMethod = DispatchInternalMethod.MakeGenericMethod(eventType);

            // Make sure they align with the method signature below
            var parameters = new[]
            {
                handlerResolver,
                commandId,
                command,
                metadata,
                cancellationToken
            };

            return await (Task<long[]>) dispatchMethod.Invoke(handlerResolver, parameters);
        }

        private static async Task<long[]> DispatchInternal<TCommand>(
            ICommandHandlerResolver handlerResolver,
            Guid commandId,
            TCommand command,
            IDictionary<string, object> metadata,
            CancellationToken cancellationToken)
            where TCommand : class
        {
            var commandMessage = new CommandMessage<TCommand>(commandId, command, metadata);
            var handlers =  handlerResolver.Resolve<TCommand>();
            if (handlers == null || !handlers.Any())
                throw new ApplicationException($"No handlers were found for command {typeof(TCommand).FullName}");

            var handlerTasks = handlers.Select(handler => handler(commandMessage, cancellationToken));
            return await Task.WhenAll(handlerTasks);
        }
    }
}
