namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Utilities.HexByteConvertor;

    public sealed class IdempotentCommandHandler : IIdempotentCommandHandler
    {
        private readonly ICommandHandlerResolver _bus;
        private readonly IdempotencyContext _idempotencyContext;

        public IdempotentCommandHandler(
            ICommandHandlerResolver bus,
            IdempotencyContext idempotencyContext)
        {
            _bus = bus;
            _idempotencyContext = idempotencyContext;
        }

        public async Task<long> Dispatch(
            Guid? commandId,
            object command,
            IDictionary<string, object> metadata,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(commandId);
            ArgumentNullException.ThrowIfNull(command);

            // First check if the command id already has been processed
            var possibleProcessedCommand = await _idempotencyContext
                .ProcessedCommands
                .Where(x => x.CommandId == commandId)
                .ToDictionaryAsync(x => x.CommandContentHash, x => x, cancellationToken);

            var contentHash = SHA512
                .Create()
                .ComputeHash(Encoding.UTF8.GetBytes(command.ToString()))
                .ToHexString();

            // It is possible we have a GUID collision, check the SHA-512 hash as well to see if it is really the same one.
            // Do nothing if commandId with contenthash exists
            if (possibleProcessedCommand.Any() && possibleProcessedCommand.ContainsKey(contentHash))
            {
                throw new IdempotencyException("Already processed");
            }

            var processedCommand = new ProcessedCommand(commandId.Value, contentHash);
            try
            {
                // Store commandId in Command Store if it does not exist
                await _idempotencyContext.ProcessedCommands.AddAsync(processedCommand, cancellationToken);
                await _idempotencyContext.SaveChangesAsync(cancellationToken);

                // Do work
                return await _bus.Dispatch(commandId.Value,
                    command,
                    metadata,
                    cancellationToken);
            }
            catch
            {
                // On exception, remove commandId from Command Store
                _idempotencyContext.ProcessedCommands.Remove(processedCommand);
                await _idempotencyContext.SaveChangesAsync(cancellationToken);
                throw;
            }
        }
    }
}
