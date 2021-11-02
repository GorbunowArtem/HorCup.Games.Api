namespace HorCup.Games.Requests
{
	public record CreateEditGameRequest(
		string Title,
		int MaxPlayers,
		int MinPlayers,
		string Description,
		string Genre);
}