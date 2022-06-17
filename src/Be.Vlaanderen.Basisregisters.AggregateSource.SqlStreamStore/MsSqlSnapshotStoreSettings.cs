namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore
{
    using System;
    using Microsoft.Data.SqlClient;

    public class MsSqlSnapshotStoreSettings
    {
        private string _schema = "dbo";
        private int _commandTimeout = 30;

        public string ConnectionString { get; }
        
        public string Schema
        {
            get => _schema;
            set => _schema = value ?? throw new ArgumentNullException(value);
        }

        /// <summary>
        ///     The log name used for any log messages.
        /// </summary>
        public string LogName { get; set; } = "MsSqlSnapshotStore";

        /// <summary>
        ///     Allows overriding the way a <see cref="SqlConnection"/> is created given a connection string.
        ///     The default implementation simply passes the connection string into the <see cref="SqlConnection"/> constructor.
        /// </summary>
        public Func<string, SqlConnection> ConnectionFactory { get; set; } = connectionString => new SqlConnection(connectionString);

        /// <summary>
        ///     Controls the wait time to execute a command. Defaults to 30 seconds.
        /// </summary>
        public int CommandTimeout
        {
            get => _commandTimeout;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("value of command timeout should be greater than 0", nameof(value));
                }

                _commandTimeout = value;
            }
        }

        /// <summary>
        ///     Initialized a new instance of <see cref="MsSqlSnapshotStoreSettings"/>.
        /// </summary>
        /// <param name="connectionString"></param>
        public MsSqlSnapshotStoreSettings(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(connectionString);
        }
    }
}
