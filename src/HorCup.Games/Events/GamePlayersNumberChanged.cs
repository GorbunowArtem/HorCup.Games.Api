namespace HorCup.Games.Events
{
	public record GamePlayersNumberChanged : DomainEvent
	{
		public int MinPlayers { get; set; }

		public int MaxPlayers { get; set; }
	}
}