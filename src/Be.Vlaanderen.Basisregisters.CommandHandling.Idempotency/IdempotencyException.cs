namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class IdempotencyException : Exception
    {
        public IdempotencyException(string? message) : base(message)
        { }

        private IdempotencyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
