using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using NEventStore;

namespace HorCup.Games.EventStore
{
	public class SqlEventStore : IEventStore
	{
		private readonly IEventPublisher _publisher;
		private readonly IStoreEvents _store;

		public SqlEventStore(IEventPublisher publisher, IStoreEvents store)
		{
			_publisher = publisher;
			_store = store;
		}

		public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
		{
			var enumerable = events.ToArray();
			using var stream = _store.OpenStream(enumerable.First().Id, 0);

			foreach (var @event in enumerable)
			{
				stream.Add(new EventMessage { Body = @event });
				await _publisher.Publish(@event, cancellationToken);
			}

			stream.CommitChanges(Guid.NewGuid());
		}

		public Task<IEnumerable<IEvent>> Get(
			Guid aggregateId,
			int fromVersion,
			CancellationToken cancellationToken = default)
		{
			var events = _store.Advanced.GetFrom(
					"default",
					aggregateId.ToString(),
					fromVersion + 1, int.MaxValue)
				.SelectMany(e => e.Events)
				.Select(e => e.Body)
				.OfType<IEvent>()
				.ToList();

			return Task.FromResult(events.AsEnumerable());
		}
	}
}