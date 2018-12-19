namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    public delegate void HandlerSync<in TMessage>(TMessage message)
        where TMessage : class;
}
