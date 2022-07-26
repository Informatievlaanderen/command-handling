namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class DomainException : Exception
    {
        protected DomainException()
        { }

        protected DomainException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        protected DomainException(string message)
            : base(message)
        { }

        protected DomainException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
