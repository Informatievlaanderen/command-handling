namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHandling;
    using global::SqlStreamStore;
    using global::SqlStreamStore.Streams;

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

        public Task<TAggregateRoot> GetAsync(TAggregateRootIdentifier identifier, CancellationToken cancellationToken = default)
            => base.GetAsync(identifier, cancellationToken);

        public Task<Optional<TAggregateRoot>> GetOptionalAsync(TAggregateRootIdentifier identifier, CancellationToken cancellationToken = default)
            => base.GetOptionalAsync(identifier, cancellationToken);

        public void Add(TAggregateRootIdentifier identifier, TAggregateRoot root)
            => base.Add(identifier, root);
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
        }

        /// <summary>
        /// Gets the aggregate root entity associated with the specified aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An instance of <typeparamref name="TAggregateRoot" />.</returns>
        /// <exception cref="T:Be.Vlaanderen.Basisregisters.AggregateSource.AggregateNotFoundException">Thrown when an aggregate is not found.</exception>
        public async Task<TAggregateRoot> GetAsync(string identifier, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await GetOptionalAsync(identifier, cancellationToken);
            if (!result.HasValue)
                throw new AggregateNotFoundException(identifier, typeof(TAggregateRoot));

            return result.Value;
        }

        /// <summary>
        /// Attempts to get the aggregate root entity associated with the aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The found <typeparamref name="TAggregateRoot" />, or empty if not found.</returns>
        public async Task<Optional<TAggregateRoot>> GetOptionalAsync(string identifier, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (UnitOfWork.TryGet(identifier, out var aggregate))
                return new Optional<TAggregateRoot>((TAggregateRoot)aggregate.Root);

            var page = await EventStore.ReadStreamForwards(identifier, StreamVersion.Start, 100, cancellationToken);

            if (page.Status == PageReadStatus.StreamNotFound)
                return Optional<TAggregateRoot>.Empty;

            var root = RootFactory();
            var events = new List<object>();
            foreach (var message in page.Messages)
            {
                var eventType = EventMapping.GetEventType(message.Type);
                var eventData = await message.GetJsonData(cancellationToken);
                events.Add(EventDeserializer.DeserializeObject(eventData, eventType));
            }
            root.Initialize(events);
            while (!page.IsEnd)
            {
                events.Clear();
                page = await page.ReadNext(cancellationToken);
                foreach (var message in page.Messages)
                {
                    var eventType = EventMapping.GetEventType(message.Type);
                    var eventData = await message.GetJsonData(cancellationToken);
                    events.Add(EventDeserializer.DeserializeObject(eventData, eventType));
                }
                root.Initialize(events);
            }
            UnitOfWork.Attach(new Aggregate(identifier, page.LastStreamVersion, root));
            return new Optional<TAggregateRoot>(root);
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
