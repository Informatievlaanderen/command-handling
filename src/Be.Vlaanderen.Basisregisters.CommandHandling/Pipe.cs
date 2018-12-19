namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    public delegate ReturnHandler<TMessage> Pipe<TMessage>(ReturnHandler<TMessage> next)
        where TMessage : class;
}
