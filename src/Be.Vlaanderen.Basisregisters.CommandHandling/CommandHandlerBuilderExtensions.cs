namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System.Threading.Tasks;

    public static class CommandHandlerBuilderExtensions
    {
        /// <summary>
        ///     Handles the message and is the last stage in a handler pipeline.
        /// </summary>
        /// <param name="handlerBuilder">The <see cref="ICommandHandlerBuilder{TMessage}"/>instance.</param>
        /// <param name="handler">The handler.</param>
        public static void Handle<TMessage>(
            this ICommandHandlerBuilder<TMessage> handlerBuilder,
            HandlerSync<TMessage> handler)
            where TMessage : class
        {
            handlerBuilder.Handle((message, _) =>
            {
                handler(message);
                return Task.FromResult(0);
            });
        }
    }
}
