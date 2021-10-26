using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Snapshotting;

namespace HorCup.Games.Tests.TestHelpers
{
	internal class SpecSnapShotStorage : ISnapshotStore
	{
		public SpecSnapShotStorage(Snapshot snapshot)
		{
			Snapshot = snapshot;
		}

		public Snapshot Snapshot { get; set; }

		public Task<Snapshot> Get(Guid id, CancellationToken cancellationToken = default)
		{
			return Task.FromResult(Snapshot);
		}

		public Task Save(Snapshot snapshot, CancellationToken cancellationToken = default)
		{
			Snapshot = snapshot;
			return Task.CompletedTask;
		}
	}
}