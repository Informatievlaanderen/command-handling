namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Tracks changes that happen to an aggregate
    /// </summary>
    public interface IAggregateChangeTracker
    {
        /// <summary>
        /// Determines whether this instance has state changes.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has state changes; otherwise, <c>false</c>.
        /// </returns>
        bool HasChanges();

        /// <summary>
        /// Gets the state changes applied to this instance.
        /// </summary>
        /// <returns>A list of recorded state changes.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<object> GetChanges();

        /// <summary>
        /// Gets the state changes applied to this instance along with metadata per change.
        /// </summary>
        /// <returns>A list of recorded state changes.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<EventWithMetadata> GetChangesWithMetadata();

        /// <summary>
        /// Clears the state changes.
        /// </summary>
        void ClearChanges();
    }
}
