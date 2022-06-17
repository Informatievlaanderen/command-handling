namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore
{
    using System;
    using System.Data;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHandling;
    using global::SqlStreamStore.Imports.Ensure.That;
    using global::SqlStreamStore.Infrastructure;
    using Microsoft.Data.SqlClient;
    using Snapshotting;

    public sealed class MsSqlSnapshotStore : ISnapshotStore
    {
        private readonly MsSqlSnapshotStoreSettings _settings;
        private readonly EventSerializer _eventSerializer;
        private readonly EventDeserializer _eventDeserializer;
        private readonly Scripts _scripts;

        protected DateTime GetUtcNow() => DateTime.UtcNow;

        public MsSqlSnapshotStore(
            MsSqlSnapshotStoreSettings settings,
            EventSerializer eventSerializer,
            EventDeserializer eventDeserializer)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _eventSerializer = eventSerializer ?? throw new ArgumentNullException(nameof(eventSerializer));
            _eventDeserializer = eventDeserializer ?? throw new ArgumentNullException(nameof(eventDeserializer));
            _scripts = new Scripts(_settings.Schema);
        }

        /// <summary>
        ///     Creates a table to hold the snapshots
        /// </summary>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CreateTable(CancellationToken cancellationToken = default)
        {
            await using var connection = _settings.ConnectionFactory(_settings.ConnectionString);
            await connection.OpenAsync(cancellationToken).NotOnCapturedContext();

            await using var command = new SqlCommand(_scripts.GetCreateTableScript(), connection);
            command.CommandTimeout = _settings.CommandTimeout;

            await command.ExecuteNonQueryAsync(cancellationToken)
                .NotOnCapturedContext();
        }

        public async Task SaveSnapshotAsync(string identifier, SnapshotContainer snapshot, CancellationToken cancellationToken)
        {
            Ensure.That(identifier, nameof(identifier)).IsNotNullOrWhiteSpace();
            Ensure.That(snapshot).IsNotNull();

            await using var connection = _settings.ConnectionFactory(_settings.ConnectionString);
            await connection.OpenAsync(cancellationToken).NotOnCapturedContext();
            var sqlStreamId = new StreamIdInfo(identifier).SqlStreamId;

            await using var command = new SqlCommand(_scripts.SaveSnapshotScript(), connection);
            command.CommandTimeout = _settings.CommandTimeout;
            command.Parameters.Add(new SqlParameter("streamId", SqlDbType.Char, 42) { Value = sqlStreamId.Id });
            command.Parameters.AddWithValue("created", GetUtcNow());
            command.Parameters.AddWithValue("snapshotBlob", _eventSerializer.SerializeObject(snapshot));

            await command.ExecuteNonQueryAsync(cancellationToken)
                .NotOnCapturedContext();
        }

        public async Task<SnapshotContainer?> FindLatestSnapshotAsync(string identifier, CancellationToken cancellationToken)
        {
            Ensure.That(identifier).IsNotNullOrWhiteSpace();

            await using var connection = _settings.ConnectionFactory(_settings.ConnectionString);
            await connection.OpenAsync(cancellationToken).NotOnCapturedContext();
            var sqlStreamId = new StreamIdInfo(identifier).SqlStreamId;

            await using var command = new SqlCommand(_scripts.GetSnapshotScript(), connection);
            command.CommandTimeout = _settings.CommandTimeout;
            command.Parameters.Add(new SqlParameter("streamId", SqlDbType.Char, 42) { Value = sqlStreamId.Id });
            command.Parameters.AddWithValue("count", 1);

            var reader = await command
                .ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancellationToken)
                .NotOnCapturedContext();

            if (!reader.HasRows)
            {
                return null;
            }

            await reader.ReadAsync(cancellationToken).NotOnCapturedContext();
            var snapshotContainerData = await reader.GetTextReader(1).ReadToEndAsync();

            return (SnapshotContainer)_eventDeserializer.DeserializeObject(snapshotContainerData,
                typeof(SnapshotContainer));
        }
    }

    //COPIED FROM SqlStreamStore => internal :(
    internal struct StreamIdInfo // Hate this name
    {
        public readonly SqlStreamId SqlStreamId;

        public readonly SqlStreamId MetadataSqlStreamId;

        public StreamIdInfo(string idOriginal)
        {
            Ensure.That(idOriginal, "streamId").IsNotNullOrWhiteSpace();

            string id;
            Guid _;
            if (Guid.TryParse(idOriginal, out _))
            {
                id = idOriginal; //If the ID is a GUID, don't bother hashing it.
            }
            else
            {
                using (var sha1 = SHA1.Create())
                {
                    var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(idOriginal));
                    id = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                }
            }
            SqlStreamId = new SqlStreamId(id, idOriginal);
            MetadataSqlStreamId = new SqlStreamId("$$" + id, "$$" + idOriginal);
        }
    }

    //COPIED FROM SqlStreamStore => internal :(
    internal struct SqlStreamId
    {
        internal readonly string Id;
        internal readonly string IdOriginal;

        public SqlStreamId(string id, string idOriginal)
        {
            Id = id;
            IdOriginal = idOriginal;
        }
    }
}
