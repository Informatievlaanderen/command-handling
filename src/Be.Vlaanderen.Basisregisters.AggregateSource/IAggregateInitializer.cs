namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    using System.Collections.Generic;

    /// <summary>
    /// Initializes an aggregate.
    /// </summary>
    public interface IAggregateInitializer
    {
        /// <summary>
        /// Initializes this instance using the specified events.
        /// </summary>
        /// <param name="events">The events to initialize with.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="events"/> are null.</exception>
        void Initialize(IEnumerable<object> events);

        /// <summary>
        /// Initializes this instance using the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot to initialize with.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="snapshot"/> is null.</exception>
        void HydrateFromSnapshot(object snapshot);
    }
}
