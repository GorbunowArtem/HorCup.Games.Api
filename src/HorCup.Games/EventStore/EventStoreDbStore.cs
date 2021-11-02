using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using EventStore.Client;
using NEventStore;

namespace HorCup.Games.EventStore
{
	public class EventStoreDbStore : IEventStore
	{
		private readonly IEventPublisher _publisher;
		private EventStoreClient _client;

		public EventStoreDbStore(IEventPublisher publisher)
		{
			_publisher = publisher;
			_client = new EventStoreClient(EventStoreClientSettings.Create("http://localhost:1113"));
		}

		public Task Save(
			IEnumerable<IEvent> events,
			CancellationToken cancellationToken = new()) =>
			 _client.AppendToStreamAsync("games-stream",
				StreamState.Any,
				events
					.Select(e => new EventData(Uuid.NewUuid(), nameof(e), JsonSerializer.SerializeToUtf8Bytes(e))),
				cancellationToken: cancellationToken);

		public async Task<IEnumerable<IEvent>> Get(
			Guid aggregateId,
			int fromVersion,
			CancellationToken cancellationToken = new())
		{
			var evetns = await _client.ReadStreamAsync(Direction.Forwards, "games-stream", StreamPosition.Start,
				cancellationToken: cancellationToken).ToListAsync(cancellationToken);

			return evetns.Select(e => e.Event)
				.OfType<IEvent>()
				.AsEnumerable();
		}
	}
}