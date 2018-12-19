namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Properties;

    /// <summary>
    /// Tracks changes of attached aggregates.
    /// </summary>
    public class ConcurrentUnitOfWork
    {
        private readonly ConcurrentDictionary<string, Aggregate> _aggregates;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentUnitOfWork"/> class.
        /// </summary>
        public ConcurrentUnitOfWork() => _aggregates = new ConcurrentDictionary<string, Aggregate>();

        /// <summary>
        /// Attaches the specified aggregate.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="aggregate"/> is null.</exception>
        public void Attach(Aggregate aggregate)
        {
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            if (!_aggregates.TryAdd(aggregate.Identifier, aggregate))
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture,
                        Resources.ConcurrentUnitOfWork_AttachAlreadyAdded,
                        aggregate.Root.GetType().Name, aggregate.Identifier));
        }

        /// <summary>
        /// Attempts to get the <see cref="Aggregate"/> using the specified aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="aggregate">The aggregate if found, otherwise <c>null</c>.</param>
        /// <returns><c>true</c> if the aggregate was found, otherwise <c>false</c>.</returns>
        public bool TryGet(string identifier, out Aggregate aggregate) => _aggregates.TryGetValue(identifier, out aggregate);

        /// <summary>
        /// Determines whether this instance has aggregates with state changes.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has aggregates with state changes; otherwise, <c>false</c>.
        /// </returns>
        public bool HasChanges() => _aggregates.Values.Any(aggregate => aggregate.Root.HasChanges());

        /// <summary>
        /// Gets the aggregates with state changes.
        /// </summary>
        /// <returns>An enumeration of <see cref="Aggregate"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IEnumerable<Aggregate> GetChanges() => _aggregates.Values.Where(aggregate => aggregate.Root.HasChanges());
    }
}
