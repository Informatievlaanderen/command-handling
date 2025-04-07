namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHandling;
    using global::SqlStreamStore;
    using global::SqlStreamStore.Streams;
    using Snapshotting;

    public class Repository<TAggregateRoot, TAggregateRootIdentifier> : Repository<TAggregateRoot>, IAsyncRepository<TAggregateRoot, TAggregateRootIdentifier>
        where TAggregateRoot : IAggregateRootEntity
        where TAggregateRootIdentifier : ValueObject<TAggregateRootIdentifier>
    {
        public Repository(
            Func<TAggregateRoot> factory,
            ConcurrentUnitOfWork unitOfWork,
            IStreamStore eventStore,
            EventMapping eventMapping,
            EventDeserializer eventDeserializer)
            : base(factory, unitOfWork, eventStore, eventMapping, eventDeserializer) { }

        public Repository(
            Func<TAggregateRoot> factory,
            ConcurrentUnitOfWork unitOfWork,
            IStreamStore eventStore,
            ISnapshotStore snapshotStore,
            EventMapping eventMapping,
            EventDeserializer eventDeserializer)
            : base(factory, unitOfWork, eventStore, eventMapping, eventDeserializer, snapshotStore) { }

        public Task<TAggregateRoot> GetAsync(TAggregateRootIdentifier identifier, CancellationToken cancellationToken = default)
            => base.GetAsync(identifier!, cancellationToken);

        public Task<Optional<TAggregateRoot>> GetOptionalAsync(TAggregateRootIdentifier identifier, CancellationToken cancellationToken = default)
            => base.GetOptionalAsync(identifier!, cancellationToken);

        public void Add(TAggregateRootIdentifier identifier, TAggregateRoot root)
            => base.Add(identifier!, root);
    }

    public class Repository<TAggregateRoot> : IAsyncRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRootEntity
    {
        /// <summary>
        /// Gets the aggregate root entity factory.
        /// </summary>
        /// <value>
        /// The aggregate root entity factory.
        /// </value>
        public Func<TAggregateRoot> RootFactory { get; }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        public ConcurrentUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Gets the event store to use.
        /// </summary>
        /// <value>
        /// The event store to use.
        /// </value>
        public IStreamStore EventStore { get; }

        /// <summary>
        /// Gets the event deserializer to use.
        /// </summary>
        /// <value>
        /// The event deserializer to use.
        /// </value>
        public EventDeserializer EventDeserializer { get; }

        /// <summary>
        /// Gets the event mapping to use.
        /// </summary>
        /// <value>
        /// The event mapping to use.
        /// </value>
        public EventMapping EventMapping { get; }

        /// <summary>
        /// Gets the snapshot store to use.
        /// </summary>
        public ISnapshotStore? SnapshotStore { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TAggregateRoot}"/> class.
        /// </summary>
        /// <param name="rootFactory">The aggregate root entity factory.</param>
        /// <param name="unitOfWork">The unit of work to interact with.</param>
        /// <param name="eventStore">The event store to use.</param>
        /// <param name="eventMapping">The event mapping.</param>
        /// <param name="eventDeserializer">The event deserializer.</param>
        /// <param name="snapshotStore">The snapshot store to use.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="rootFactory"/> or <paramref name="unitOfWork"/> or <paramref name="eventStore"/> or <paramref name="snapshotStore"/> is null.</exception>
        public Repository(Func<TAggregateRoot> rootFactory, ConcurrentUnitOfWork unitOfWork, IStreamStore eventStore, EventMapping eventMapping, EventDeserializer eventDeserializer, ISnapshotStore snapshotStore)
        {
            RootFactory = rootFactory ?? throw new ArgumentNullException(nameof(rootFactory));
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            EventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            EventMapping = eventMapping ?? throw new ArgumentNullException(nameof(eventMapping));
            EventDeserializer = eventDeserializer ?? throw new ArgumentNullException(nameof(eventDeserializer));
            SnapshotStore = snapshotStore ?? throw new ArgumentNullException(nameof(snapshotStore));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TAggregateRoot}"/> class.
        /// </summary>
        /// <param name="rootFactory">The aggregate root entity factory.</param>
        /// <param name="unitOfWork">The unit of work to interact with.</param>
        /// <param name="eventStore">The event store to use.</param>
        /// <param name="eventMapping">The event mapping.</param>
        /// <param name="eventDeserializer">The event deserializer.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="rootFactory"/> or <paramref name="unitOfWork"/> or <paramref name="eventStore"/> is null.</exception>
        public Repository(Func<TAggregateRoot> rootFactory, ConcurrentUnitOfWork unitOfWork, IStreamStore eventStore, EventMapping eventMapping, EventDeserializer eventDeserializer)
        {
            RootFactory = rootFactory ?? throw new ArgumentNullException(nameof(rootFactory));
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            EventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            EventMapping = eventMapping ?? throw new ArgumentNullException(nameof(eventMapping));
            EventDeserializer = eventDeserializer ?? throw new ArgumentNullException(nameof(eventDeserializer));
            SnapshotStore = null;
        }

        /// <summary>
        /// Gets the aggregate root entity associated with the specified aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An instance of <typeparamref name="TAggregateRoot" />.</returns>
        /// <exception cref="T:Be.Vlaanderen.Basisregisters.AggregateSource.AggregateNotFoundException">Thrown when an aggregate is not found.</exception>
        public async Task<TAggregateRoot> GetAsync(string identifier, CancellationToken cancellationToken = default)
        {
            var result = await GetOptionalAsync(identifier, cancellationToken);
            if (!result.HasValue)
            {
                throw new AggregateNotFoundException(identifier, typeof(TAggregateRoot));
            }

            return result.Value;
        }

        /// <summary>
        /// Attempts to get the aggregate root entity associated with the aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The found <typeparamref name="TAggregateRoot" />, or empty if not found.</returns>
        public async Task<Optional<TAggregateRoot>> GetOptionalAsync(string identifier, CancellationToken cancellationToken = default)
        {
            // Check if the aggregate is already created and present in the UoW
            if (UnitOfWork.TryGet(identifier, out var aggregate))
            {
                return new Optional<TAggregateRoot>((TAggregateRoot)aggregate.Root);
            }

            var snapshotSupport = typeof(ISnapshotable).IsAssignableFrom(typeof(TAggregateRoot));

            if (!snapshotSupport)
            {
                return await GetAggregateAsync(identifier, StreamVersion.Start, null, cancellationToken);
            }

            var hydrateContainer = new HydrateContainer();
            if (SnapshotStore is not null)
            {
                var snapshotContainer = await SnapshotStore.FindLatestSnapshotAsync(identifier, cancellationToken);
                if (snapshotContainer is not null)
                {
                    var snapshotType = EventMapping.GetEventType(snapshotContainer.Info.Type);
                    var snapshotData = snapshotContainer.Data;

                    hydrateContainer.ReadFrom =
                        (int)snapshotContainer.Info.StreamVersion + 1; // TODO: Fix this when SSS supports longs for read
                    hydrateContainer.Snapshot = EventDeserializer.DeserializeObject(snapshotData, snapshotType);
                }
            }
            else
            {
                // Otherwise, check if there is a snapshot in the event store and hydrate from there
                var snapshotIdentifier = UnitOfWork.GetSnapshotIdentifier(identifier);
                var snapshotPage = await EventStore.ReadStreamBackwards(
                    snapshotIdentifier,
                    StreamVersion.End,
                    1,
                    false,
                    cancellationToken);


                if (snapshotPage.Status != PageReadStatus.StreamNotFound)
                {
                    var snapshotContainerMessage = snapshotPage.Messages.Single();
                    var snapshotContainerData =
                        await snapshotContainerMessage
                            .GetJsonData(cancellationToken); // { info: { position: xxx, type: "" }, data: "" }
                    var snapshotContainer =
                        (SnapshotContainer)EventDeserializer.DeserializeObject(snapshotContainerData,
                            typeof(SnapshotContainer));

                    var snapshotType = EventMapping.GetEventType(snapshotContainer.Info.Type);
                    var snapshotData = snapshotContainer.Data;

                    hydrateContainer.ReadFrom =
                        (int)snapshotContainer.Info.StreamVersion + 1; // TODO: Fix this when SSS supports longs for read
                    hydrateContainer.Snapshot = EventDeserializer.DeserializeObject(snapshotData, snapshotType);
                }
            }

            void HydrateFromSnapshot(TAggregateRoot root)
            {
                if (root is ISnapshotable s && hydrateContainer.Snapshot != null)
                {
                    s.RestoreSnapshot(hydrateContainer.Snapshot);
                }
            }

            return await GetAggregateAsync(
                identifier,
                hydrateContainer.ReadFrom,
                HydrateFromSnapshot,
                cancellationToken);
        }

        private async Task<Optional<TAggregateRoot>> GetAggregateAsync(
            string identifier,
            int readFrom,
            Action<TAggregateRoot>? restoreSnapshot,
            CancellationToken cancellationToken)
        {
            // Apply events from start, or from the snapshot position
            var page = await EventStore.ReadStreamForwards(identifier, readFrom, 100, cancellationToken);

            if (page.Status == PageReadStatus.StreamNotFound)
            {
                return Optional<TAggregateRoot>.Empty;
            }

            var root = RootFactory();
            restoreSnapshot?.Invoke(root);

            await ParseEvents(root, page, cancellationToken);
            while (!page.IsEnd)
            {
                page = await page.ReadNext(cancellationToken);
                await ParseEvents(root, page, cancellationToken);
            }

            UnitOfWork.Attach(new Aggregate(identifier, page.LastStreamVersion, root));

            return new Optional<TAggregateRoot>(root);
        }

        private async Task ParseEvents(
            TAggregateRoot root,
            ReadStreamPage page,
            CancellationToken cancellationToken)
        {
            var events = new List<object>();

            foreach (var message in page.Messages)
            {
                var eventType = EventMapping.GetEventType(message.Type);
                var eventData = await message.GetJsonData(cancellationToken);
                events.Add(EventDeserializer.DeserializeObject(eventData, eventType));
            }

            root.Initialize(events);
        }

        /// <summary>
        /// Adds the aggregate root entity to this collection using the specified aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="root">The aggregate root entity.</param>
        public void Add(string identifier, TAggregateRoot root)
            => UnitOfWork.Attach(new Aggregate(identifier, ExpectedVersion.NoStream, root));
    }
}
