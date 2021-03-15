namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base class for aggregate root entities that need some basic infrastructure for tracking state changes.
    /// </summary>
    public abstract class AggregateRootEntity : IAggregateRootEntity
    {
        private const int SnapshotIntervalDefault = 1000;

        private readonly EventRecorder _recorder;
        private readonly IConfigureInstanceEventRouter _router;

        public virtual bool EnableSnapshots => false;

        public virtual int SnapshotInterval => SnapshotIntervalDefault;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRootEntity"/> class.
        /// </summary>
        protected AggregateRootEntity()
        {
            _router = new InstanceEventRouter();
            _recorder = new EventRecorder();
        }

        /// <summary>
        /// Registers the state handler to be invoked when the specified event is applied.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to register the handler for.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="handler"/> is null.</exception>
        protected void Register<TEvent>(Action<TEvent> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            _router.ConfigureRoute(handler);
        }

        /// <summary>
        /// Initializes this instance using the specified events.
        /// </summary>
        /// <param name="events">The events to initialize with.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown when the <paramref name="events" /> are null.</exception>
        public void Initialize(IEnumerable<object> events)
        {
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            if (HasChanges())
                throw new InvalidOperationException("Initialize cannot be called on an instance with changes.");

            foreach (var @event in events)
                Play(@event);
        }

        public void HydrateFromSnapshot(object snapshot)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));

            if (HasChanges())
                throw new InvalidOperationException("HydrateFromSnapshot cannot be called on an instance with changes.");

            Play(snapshot);
        }

        /// <summary>
        /// Applies the specified event to this instance and invokes the associated state handler.
        /// </summary>
        /// <param name="event">The event to apply.</param>
        protected void ApplyChange(object @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            BeforeApplyChange(@event);
            Play(@event);
            Record(@event);
            AfterApplyChange(@event);
        }

        /// <summary>
        /// Called before an event is applied, exposed as a point of interception.
        /// </summary>
        /// <param name="event">The event that will be applied.</param>
        protected virtual void BeforeApplyChange(object @event) {}

        /// <summary>
        /// Called after an event has been applied, exposed as a point of interception.
        /// </summary>
        /// <param name="event">The event that has been applied.</param>
        protected virtual void AfterApplyChange(object @event) {}

        private void Play(object @event) => _router.Route(@event);

        private void Record(object @event)
        {
            // TODO: Next step, move away from ambient data
            _recorder.Record(EventMetadataContext.MetadataAsync != null && EventMetadataContext.MetadataAsync.Value != null
                ? new EventWithMetadata(@event, EventMetadataContext.MetadataAsync.Value)
                : @event);
        }

        /// <summary>
        /// Determines whether this instance has state changes.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has state changes; otherwise, <c>false</c>.
        /// </returns>
        public bool HasChanges() => _recorder.Any();

        /// <summary>
        /// Gets the state changes applied to this instance.
        /// </summary>
        /// <returns>A list of recorded state changes.</returns>
        public IEnumerable<object> GetChanges() =>
            _recorder
                .Select(e => e is EventWithMetadata ? ((EventWithMetadata)e).Event : e)
                .ToArray();

        public IEnumerable<EventWithMetadata> GetChangesWithMetadata() =>
            _recorder
                .Select(e => e is EventWithMetadata ? e : new EventWithMetadata(e)).Cast<EventWithMetadata>()
                .ToArray();


        public virtual object CreateSnapshot() => null;

        /// <summary>
        /// Clears the state changes.
        /// </summary>
        public void ClearChanges() => _recorder.Reset();
    }
}
