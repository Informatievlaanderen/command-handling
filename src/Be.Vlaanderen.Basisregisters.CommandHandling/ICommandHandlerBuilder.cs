namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    /// <summary>
    ///     Provides a mechanism to fluently build a command handler pipeline.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message the handler will handle.</typeparam>
    public interface ICommandHandlerBuilder<TMessage> where TMessage : class
    {
        /// <summary>
        /// Pipes the message through handler middleware.
        /// </summary>
        /// <param name="pipe">The next handler middleware to invoke.</param>
        /// <returns>The <see cref="ICommandHandlerBuilder{TMessage}"/> instance.</returns>
        ICommandHandlerBuilder<TMessage> Pipe(Pipe<TMessage> pipe);

        /// <summary>
        /// Handles the message and is the last stage in a handler pipeline.
        /// </summary>
        /// <param name="handler">The handler.</param>
        void Handle(Handler<TMessage> handler);

        ReturnHandler<TMessage> Finally(ReturnHandler<TMessage> finalHandler);
    }
}
