namespace Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting.InMemory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     Represents an in-memory implementation of a snapshot store. Use for testing.
    /// </summary>
    public sealed class InMemorySnapshotStore : ISnapshotStore, IDisposable
    {
        private readonly Func<DateTime> _getUtcNow;
        private readonly Dictionary<string, List<InMemorySnapshot>> _snapshots = new Dictionary<string, List<InMemorySnapshot>>();
        private int _identity = 0;

        public InMemorySnapshotStore()
        {
            _getUtcNow = () => DateTime.UtcNow;
        }

        public Task SaveSnapshotAsync(string identifier, SnapshotContainer snapshot, CancellationToken cancellationToken)
        {
            _identity++;

            if (!_snapshots.ContainsKey(identifier))
            {
                _snapshots.Add(identifier, new List<InMemorySnapshot>());
            }

            _snapshots[identifier].Add(new InMemorySnapshot(_identity, identifier, _getUtcNow(), snapshot));

            return Task.CompletedTask;
        }

        public Task<SnapshotContainer?> FindLatestSnapshotAsync(string identifier, CancellationToken cancellationToken)
        {
            if (!_snapshots.ContainsKey(identifier))
            {
                return Task.FromResult((SnapshotContainer?)null);
            }

            var lastSnapshot = _snapshots[identifier]
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            return Task.FromResult(lastSnapshot?.Snapshot);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            _snapshots.Clear();
        }

        ~InMemorySnapshotStore()
        {
            Dispose(false);
        }
    }
}
