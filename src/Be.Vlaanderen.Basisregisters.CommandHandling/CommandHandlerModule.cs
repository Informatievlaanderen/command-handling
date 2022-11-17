namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandHandlerModule
    {
        internal HashSet<CommandHandlerRegistration> HandlerRegistrations { get; }
            = new HashSet<CommandHandlerRegistration>(CommandHandlerRegistration.MessageTypeComparer);

        public virtual ICommandHandlerBuilder<CommandMessage<TCommand>> For<TCommand>()
            where TCommand : class
            => new CommandHandlerBuilder<TCommand>(handlerRegistration =>
            {
                if (!HandlerRegistrations.Add(handlerRegistration))
                {
                    throw new InvalidOperationException(
                        "Attempt to register multiple handlers for command type {0}".FormatWith(typeof(TCommand)));
                }
            });

        public IEnumerable<Type> CommandTypes => HandlerRegistrations.Select(r => r.CommandType);

        private sealed class CommandHandlerBuilder<TCommand> : ICommandHandlerBuilder<CommandMessage<TCommand>>
            where TCommand : class
        {
            private readonly Stack<Pipe<CommandMessage<TCommand>>> _pipes = new Stack<Pipe<CommandMessage<TCommand>>>();
            private readonly Action<CommandHandlerRegistration> _registerHandler;
            private Handler<CommandMessage<TCommand>>? _handler;

            internal CommandHandlerBuilder(Action<CommandHandlerRegistration> registerHandler) => _registerHandler = registerHandler;

            public ICommandHandlerBuilder<CommandMessage<TCommand>> Pipe(Pipe<CommandMessage<TCommand>> pipe)
            {
                _pipes.Push(pipe);
                return this;
            }

            public void Handle(Handler<CommandMessage<TCommand>>? handler)
            {
                _handler = handler;

                ReturnHandler<CommandMessage<TCommand>>? composed = null;
                if (_pipes.Count == 0)
                {
                    composed = async (msg, ct) =>
                    {
                        if (_handler is not null)
                        {
                            await _handler(msg, ct);
                        }

                        return -1L;
                    };
                }
                else
                {
                    while (_pipes.Count > 0)
                    {
                        var pipe = _pipes.Pop();
                        composed ??= async (msg, ct) =>
                        {
                            if (_handler != null)
                            {
                                await _handler(msg, ct);
                            }

                            return -1L;
                        };

                        composed = pipe(composed);
                    }
                }

                if (composed is not null)
                {
                    Register(async (msg, ct) => await composed.Invoke(msg, ct));
                }
            }

            private void Register(ReturnHandler<CommandMessage<TCommand>> fullHandler)
            {
                var registrationType = typeof(ReturnHandler<CommandMessage<TCommand>>);

                _registerHandler(new CommandHandlerRegistration(
                    typeof(TCommand),
                    registrationType,
                    fullHandler));
            }
        }
    }
}
