using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;

namespace HorCup.Games.Tests.TestHelpers
{
	internal class SpecEventStorage : IEventStore
	{
		private readonly IEventPublisher _publisher;

		public SpecEventStorage(IEventPublisher publisher, List<IEvent> events)
		{
			_publisher = publisher;
			Events = events;
		}

		public List<IEvent> Events { get; set; }

		public Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
		{
			Events.AddRange(events);
			return Task.WhenAll(events.Select(evt =>_publisher.Publish<IEvent>(evt, cancellationToken)));
		}

		public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default)
		{
			var events = Events.Where(x => x.Id == aggregateId && x.Version > fromVersion);
			return Task.FromResult(events);
		}
	}
}