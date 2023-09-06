namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    public interface ISnapshotVerificationNotifier
    {
        void NotifyInvalidSnapshot(int snapshotId, string differences);
    }
}
