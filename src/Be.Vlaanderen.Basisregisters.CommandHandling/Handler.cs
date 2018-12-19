namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System.Threading;
    using System.Threading.Tasks;

    public delegate Task Handler<in TMessage>(TMessage message, CancellationToken ct)
        where TMessage: class;
}
