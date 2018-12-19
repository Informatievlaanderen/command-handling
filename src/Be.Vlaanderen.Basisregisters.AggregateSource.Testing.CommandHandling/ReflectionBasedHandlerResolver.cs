namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.CommandHandling
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.CommandHandling;

    public class ReflectionBasedHandlerResolver : IHandlerResolver
    {
        private readonly ICommandHandlerResolver _commandHandlerResolver;

        public ReflectionBasedHandlerResolver(ICommandHandlerResolver commandHandlerResolver)
            => _commandHandlerResolver = commandHandlerResolver ?? throw new ArgumentNullException(nameof(commandHandlerResolver));

        public Func<object, Task<long>> ResolveHandlerFor(object command)
        {
            //a lot of reflection here, perhaps CommandHandling library could offer a less type dependent way of handling commands...?
            var resolveHandlerMethod = typeof(ICommandHandlerResolver).GetRuntimeMethod("Resolve", new Type[0]).MakeGenericMethod(command.GetType());
            var handler = resolveHandlerMethod.Invoke(_commandHandlerResolver, new object[0]);
            var commandMessageType = typeof(CommandMessage<>).MakeGenericType(command.GetType());
            var invokeHandler = handler.GetType().GetRuntimeMethod("Invoke", new[] { commandMessageType, typeof(CancellationToken) });
            var commandMessageConstructor = commandMessageType.GetConstructor(new[] { typeof(Guid), command.GetType(), typeof(IDictionary<string, object>) });
            object CommandMessageFactory(object cmd) => commandMessageConstructor.Invoke(new[] { Guid.NewGuid(), cmd, new Dictionary<string, object> { { "MessagePurpose", "Testing" } } });
            return async cmd => await (Task<long>)invokeHandler.Invoke(handler, new[] { CommandMessageFactory(cmd), CancellationToken.None });
        }
    }
}
