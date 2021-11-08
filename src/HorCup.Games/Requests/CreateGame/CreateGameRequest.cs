namespace HorCup.Games.Requests.CreateGame
{
	public record CreateGameRequest(
		string Title,
		int MaxPlayers,
		int MinPlayers,
		string Description,
		string Genre);
}