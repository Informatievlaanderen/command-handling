namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    public class SnapshotVerificationState
    {
        public int SnapshotId { get; }
        public SnapshotStateStatus Status { get; set; }
        public string? Differences { get; set; }

        public SnapshotVerificationState(int snapshotId)
        {
            SnapshotId = snapshotId;
            Status = SnapshotStateStatus.Failed;
        }
    }
}
