using System;
using CQRSlite.Events;

namespace HorCup.Games.Events
{
	public record DomainEvent: IEvent
	{
		public Guid Id { get; set; }
		public int Version { get; set; }
		public DateTimeOffset TimeStamp { get; set; }
	}
}