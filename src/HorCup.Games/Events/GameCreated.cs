namespace HorCup.Games.Events
{
	public record GameCreated: DomainEvent
	{
		public string Genre { get; set; }
		
		public int MinPlayers { get; set; }

		public int MaxPlayers { get; set; }
	}
}