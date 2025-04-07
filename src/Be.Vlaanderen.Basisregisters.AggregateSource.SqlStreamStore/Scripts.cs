namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore
{
    using System;

    public class Scripts
    {
        private readonly string _schema;

        public Scripts(string? schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public string GetCreateTableScript() => @$"
IF object_id('{_schema}.Snapshots', 'U') IS NULL
BEGIN
    CREATE TABLE {_schema}.Snapshots(
        Id                  INT                                     NOT NULL IDENTITY (1, 1),
        StreamId            CHAR(42)                                NOT NULL,
        Created             DATETIME                                NOT NULL,
        SnapshotBlob        NVARCHAR(max)                           NOT NULL,
        CONSTRAINT PK_Snapshots PRIMARY KEY (Id)
    );
END

IF NOT EXISTS(
    SELECT *
    FROM sys.indexes
    WHERE name='IX_Snapshots_StreamId_Id' AND object_id = OBJECT_ID('{_schema}.Snapshots'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX IX_Snapshots_StreamId_Id ON {_schema}.Snapshots (StreamId, Id);
END
";

        public string SaveSnapshotScript() => $@"
BEGIN TRANSACTION SaveSnapshot;

    INSERT INTO {_schema}.Snapshots (StreamId, Created, SnapshotBlob)
    VALUES (@streamId, @created, @snapshotBlob)

COMMIT TRANSACTION SaveSnapshot;
";

        public string GetSnapshotScript() => $@"
SELECT TOP(@count)
            {_schema}.Snapshots.Created,
            {_schema}.Snapshots.SnapshotBlob
       FROM {_schema}.Snapshots
      WHERE {_schema}.Snapshots.StreamId = @streamId
   ORDER BY {_schema}.Snapshots.Id DESC;
";
    }
}
