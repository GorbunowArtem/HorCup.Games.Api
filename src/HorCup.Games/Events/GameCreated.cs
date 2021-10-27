namespace HorCup.Games.Events
{
	public record GameCreated: DomainEvent
	{
		public string Genre { get; set; }
	}
}