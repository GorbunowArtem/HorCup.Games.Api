namespace HorCup.Games.Events
{
	public record GameTitleSet : DomainEvent
	{
		public string Title { get; set; }
	}
}