using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using NEventStore;
using NEventStore.PollingClient;

namespace HorCup.Games.Services.Rebuild
{
	public class ProjectionRebuildService : IProjectionRebuildService
	{
		private readonly IEventPublisher _publisher;
		private readonly IStoreEvents _store;
		private object _lock = new object();

		public ProjectionRebuildService(IStoreEvents store, IEventPublisher publisher)
		{
			_store = store;
			_publisher = publisher;
		}

		public async Task Execute()
		{
			lock (_lock)
			{
				var client = new PollingClient2(_store.Advanced, commit =>
					{
						foreach (var eventMessage in commit.Events)
						{
							_publisher.Publish(eventMessage.Body as IEvent);
							Thread.Sleep(500);
						}

						return PollingClient2.HandlingResult.MoveToNext;
					},
					waitInterval: 3000);

				client.StartFrom(0);
			}
		}
	}
}