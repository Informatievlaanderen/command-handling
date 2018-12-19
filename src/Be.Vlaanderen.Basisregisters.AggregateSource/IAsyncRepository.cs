namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an asynchronous, virtual collection of <typeparamref name="TAggregateRoot"/>.
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of the aggregate root in this collection.</typeparam>
    public interface IAsyncRepository<TAggregateRoot>
    {
        /// <summary>
        /// Gets the aggregate root entity associated with the specified aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An instance of <typeparamref name="TAggregateRoot"/>.</returns>
        /// <exception cref="AggregateNotFoundException">Thrown when an aggregate is not found.</exception>
        Task<TAggregateRoot> GetAsync(string identifier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Attempts to get the aggregate root entity associated with the aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The found <typeparamref name="TAggregateRoot"/>, or empty if not found.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        Task<Optional<TAggregateRoot>> GetOptionalAsync(string identifier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the aggregate root entity to this collection using the specified aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="root">The aggregate root entity.</param>
        void Add(string identifier, TAggregateRoot root);
    }

    /// <summary>
    /// Represents an asynchronous, virtual collection of <typeparamref name="TAggregateRoot"/>.
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of the aggregate root in this collection.</typeparam>
    /// <typeparam name="TAggregateRootIdentifier">The type of the identifier for the aggregate root in this collection.</typeparam>
    public interface IAsyncRepository<TAggregateRoot, in TAggregateRootIdentifier>
    {
        /// <summary>
        /// Gets the aggregate root entity associated with the specified aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An instance of <typeparamref name="TAggregateRoot"/>.</returns>
        /// <exception cref="AggregateNotFoundException">Thrown when an aggregate is not found.</exception>
        Task<TAggregateRoot> GetAsync(TAggregateRootIdentifier identifier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Attempts to get the aggregate root entity associated with the aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The found <typeparamref name="TAggregateRoot"/>, or empty if not found.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        Task<Optional<TAggregateRoot>> GetOptionalAsync(TAggregateRootIdentifier identifier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the aggregate root entity to this collection using the specified aggregate identifier.
        /// </summary>
        /// <param name="identifier">The aggregate identifier.</param>
        /// <param name="root">The aggregate root entity.</param>
        void Add(TAggregateRootIdentifier identifier, TAggregateRoot root);
    }
}
