namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISnapshotVerificationRepository
    {
        void EnsureCreated();
        Task<int?> MaxSnapshotId(CancellationToken cancellationToken = default);

        Task AddVerificationState(SnapshotVerificationState verificationState,
            CancellationToken cancellationToken = default);
    }
}
