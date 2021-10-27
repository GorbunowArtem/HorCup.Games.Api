using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;

namespace HorCup.Games.Tests.TestHelpers
{
	internal class SpecEventPublisher : IEventPublisher
	{
		public SpecEventPublisher()
		{
			PublishedEvents = new List<IEvent>();
		}

		public Task Publish<T>(T @event, CancellationToken cancellationToken = default) where T : class, IEvent
		{
			PublishedEvents.Add(@event);
			return Task.CompletedTask;
		}

		public IList<IEvent> PublishedEvents { get; set; }
	}
}