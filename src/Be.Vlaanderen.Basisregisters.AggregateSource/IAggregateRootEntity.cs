namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    /// <inheritdoc cref="IAggregateInitializer" />
    /// <summary>
    /// Aggregate root entity marker interface.
    /// </summary>
    public interface IAggregateRootEntity : IAggregateInitializer, IAggregateChangeTracker {}
}
