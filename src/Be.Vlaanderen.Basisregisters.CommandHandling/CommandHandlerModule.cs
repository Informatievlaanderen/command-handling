namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CommandHandlerModule
    {
        private readonly ReturnHandler<CommandMessage> _finalHandler;

        internal HashSet<CommandHandlerRegistration> HandlerRegistrations { get; }
            = new HashSet<CommandHandlerRegistration>(CommandHandlerRegistration.MessageTypeComparer);

        public CommandHandlerModule(ReturnHandler<CommandMessage> finalHandler = null) => _finalHandler = finalHandler;

        protected void Wrap(CommandHandlerModule commandHandlerModule)
        {
            foreach (var registration in commandHandlerModule.HandlerRegistrations.ToList())
                HandlerRegistrations.Add(registration);
        }

        public virtual ICommandHandlerBuilder<CommandMessage<TCommand>> For<TCommand>()
            where TCommand : class
        {
            return new CommandHandlerBuilder<TCommand>(handlerRegistration =>
            {
                if (!HandlerRegistrations.Add(handlerRegistration))
                    throw new InvalidOperationException("Attempt to register multiple handlers for command type {0}".FormatWith(typeof(TCommand)));
            }, _finalHandler);
        }

        public IEnumerable<Type> CommandTypes => HandlerRegistrations.Select(r => r.CommandType);

        private class CommandHandlerBuilder<TCommand> : ICommandHandlerBuilder<CommandMessage<TCommand>>
            where TCommand : class
        {
            private readonly Stack<Pipe<CommandMessage<TCommand>>> _pipes = new Stack<Pipe<CommandMessage<TCommand>>>();
            private readonly Action<CommandHandlerRegistration> _registerHandler;
            private readonly ReturnHandler<CommandMessage<TCommand>> _finalHandler;
            private Handler<CommandMessage<TCommand>> _handler;

            internal CommandHandlerBuilder(Action<CommandHandlerRegistration> registerHandler, ReturnHandler<CommandMessage> finalHandler)
            {
                _registerHandler = registerHandler;
                _finalHandler = finalHandler;
            }

            public ICommandHandlerBuilder<CommandMessage<TCommand>> Pipe(Pipe<CommandMessage<TCommand>> pipe)
            {
                _pipes.Push(pipe);
                return this;
            }

            public void Handle(Handler<CommandMessage<TCommand>> handler)
            {
                _handler = handler;
                var finalHandler = _finalHandler ?? ((msg, ct) => Task.FromResult(-1L));

                ReturnHandler<CommandMessage<TCommand>> composed = null;
                if (_pipes.Count == 0)
                {
                    composed = async (msg, ct) =>
                    {
                        if (_handler != null)
                            await _handler(msg, ct);

                        return 0L;
                    };
                }
                else
                {
                    while (_pipes.Count > 0)
                    {
                        var pipe = _pipes.Pop();
                        if (composed == null)
                        {
                            composed = async (msg, ct) =>
                            {
                                if (_handler != null)
                                    await _handler(msg, ct);

                                return 0L;
                            };
                        }

                        composed = pipe(composed);
                    }
                }

                Register(async (msg, ct) =>
                {
                    await composed(msg, ct);

                    return await finalHandler(msg, ct);
                });
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
