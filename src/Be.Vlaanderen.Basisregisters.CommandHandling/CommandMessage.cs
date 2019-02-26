namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandMessage
    {
        public readonly Guid CommandId;
        public readonly IDictionary<string, object> Metadata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessage"/> class.
        /// </summary>
        /// <param name="commandId">The command identifier.</param>
        /// <param name="metadata">The metadata.</param>
        public CommandMessage(
            Guid commandId,
            IDictionary<string, object> metadata = null)
        {
            CommandId = commandId;

            if (metadata != null)
                Metadata = metadata.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }

    /// <summary>
    /// Represents a command with associated metadata
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    public sealed class CommandMessage<TCommand> : CommandMessage
    {
        public readonly TCommand Command;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Be.Vlaanderen.Basisregisters.CommandHandling.CommandMessage`1" /> class.
        /// </summary>
        /// <param name="commandId">The command identifier.</param>
        /// <param name="command">The command.</param>
        /// <param name="metadata">The metadata.</param>
        public CommandMessage(
            Guid commandId,
            TCommand command,
            IDictionary<string, object> metadata = null) : base(commandId, metadata) => Command = command;
    }
}
