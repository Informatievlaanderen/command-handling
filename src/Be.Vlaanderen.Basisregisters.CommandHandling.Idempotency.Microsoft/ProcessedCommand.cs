namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Microsoft
{
    using System;

    public class ProcessedCommand
    {
        public Guid CommandId { get; set; }

        public string CommandContentHash { get; set; }

        public DateTimeOffset DateProcessed { get; set; }

        public ProcessedCommand() { }

        public ProcessedCommand(Guid commandId, string sha)
        {
            CommandId = commandId;
            CommandContentHash = sha;
            DateProcessed = DateTimeOffset.Now;
        }
    }
}
