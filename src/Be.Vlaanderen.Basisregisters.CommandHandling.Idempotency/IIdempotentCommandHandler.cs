namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IIdempotentCommandHandler
    {
        Task<long> Dispatch(
            Guid? commandId,
            object command,
            IDictionary<string, object> metadata,
            CancellationToken cancellationToken);
    }
}
