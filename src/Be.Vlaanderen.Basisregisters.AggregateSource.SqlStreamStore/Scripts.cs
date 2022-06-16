namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore
{
    public class Scripts
    {
        private readonly string _schema;

        public Scripts(string schema)
        {
            _schema = schema;
        }

        public string GetCreateTableScript() => @$"
IF object_id('{_schema}.Snapshots', 'U') IS NULL
BEGIN
    CREATE TABLE {_schema}.Snapshots(
        Id                  INT                                     NOT NULL IDENTITY (1, 1),
        StreamIdInternal    INT                                     NOT NULL,
        Created             DATETIME                                NOT NULL,
        SnapshotBlob        NVARCHAR(max)                           NOT NULL,
        CONSTRAINT PK_Snapshots PRIMARY KEY (Id),
        CONSTRAINT FK_Snapshots_Streams FOREIGN KEY (StreamIdInternal) REFERENCES {_schema}.Streams(IdInternal)
    );
END

IF NOT EXISTS(
    SELECT * 
    FROM sys.indexes
    WHERE name='IX_Snapshots_StreamIdInternal_Id' AND object_id = OBJECT_ID('{_schema}.Snapshots'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX IX_Snapshots_StreamIdInternal_Id ON {_schema}.Snapshots (StreamIdInternal, Id);
END
";

        public string SaveSnapshotScript() => $@"
BEGIN TRANSACTION SaveSnapshot;

    DECLARE @streamIdInternal AS INT;
    DECLARE @latestStreamVersion AS INT;
    DECLARE @latestStreamPosition AS BIGINT;

    SELECT @streamIdInternal = {_schema}.Streams.IdInternal, @latestStreamVersion = {_schema}.Streams.[Version]
    FROM {_schema}.Streams WITH (UPDLOCK, ROWLOCK)
    WHERE {_schema}.Streams.Id = @streamId;

    INSERT INTO {_schema}.Snapshots (StreamIdInternal, Created, SnapshotBlob)
    VALUES (@streamIdInternal, @created, @snapshotBlob)

COMMIT TRANSACTION SaveSnapshot;
";

        public string GetSnapshotScript() => $@"
SELECT TOP(@count)
            {_schema}.Streams.IdOriginal As StreamId,
            {_schema}.Snapshots.Created,            
            {_schema}.Snapshots.SnapshotBlob
       FROM {_schema}.Snapshots
 INNER JOIN {_schema}.Streams
         ON {_schema}.Snapshots.StreamIdInternal = {_schema}.Streams.IdInternal
      WHERE {_schema}.Streams.Id = @streamId
   ORDER BY {_schema}.Snapshots.Id DESC;
";
    }
}
