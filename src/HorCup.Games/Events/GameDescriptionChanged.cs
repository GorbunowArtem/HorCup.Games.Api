namespace HorCup.Games.Events
{
	public record GameDescriptionChanged : DomainEvent
	{
		public string Description { get; set; }
	}
}