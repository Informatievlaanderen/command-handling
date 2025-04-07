namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.Serialization;
    using Properties;

    /// <summary>
    /// Exception that tells callers an aggregate was not found.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [Serializable]
    public class AggregateNotFoundException : AggregateSourceException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Be.Vlaanderen.Basisregisters.AggregateSource.AggregateNotFoundException" /> class.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="clrType">ClrType of the aggregate root entity.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown when the <paramref name="clrType" /> is null.</exception>
        public AggregateNotFoundException(string? identifier, Type? clrType)
            : base(
                clrType != null && identifier != null
                    ? string.Format(CultureInfo.InvariantCulture, Resources.AggregateNotFoundException_DefaultMessage, clrType.Name, identifier)
                    : null)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            ClrType = clrType ?? throw new ArgumentNullException(nameof(clrType));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Be.Vlaanderen.Basisregisters.AggregateSource.AggregateNotFoundException" /> class.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="clrType">ClrType of the aggregate root entity.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown when the <paramref name="clrType" /> is null.</exception>
        public AggregateNotFoundException(string identifier, Type clrType, string message)
            : base(message)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            ClrType = clrType ?? throw new ArgumentNullException(nameof(clrType));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Be.Vlaanderen.Basisregisters.AggregateSource.AggregateNotFoundException" /> class.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="clrType">ClrType of the aggregate root entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown when the <paramref name="clrType" /> is null.</exception>
        public AggregateNotFoundException(string identifier, Type clrType, string message, Exception innerException)
            : base(message, innerException)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            ClrType = clrType ?? throw new ArgumentNullException(nameof(clrType));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Be.Vlaanderen.Basisregisters.AggregateSource.AggregateNotFoundException" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected AggregateNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Identifier = info.GetString("identifier");
            ClrType = Type.GetType(info.GetString("clrType"), false);
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        ///   </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("identifier", Identifier);
            info.AddValue("clrType", ClrType?.AssemblyQualifiedName);
        }

        /// <summary>
        /// Gets the aggregate id.
        /// </summary>
        /// <value>
        /// The aggregate id.
        /// </value>
        public string Identifier { get; }

        /// <summary>
        /// Gets the <see cref="System.Type">ClrType</see> of the aggregate root entity.
        /// </summary>
        /// <value>
        /// The ClrType of the aggregate root entity, or <c>null</c> if type not found.
        /// </value>
        public Type? ClrType { get; }
    }
}
