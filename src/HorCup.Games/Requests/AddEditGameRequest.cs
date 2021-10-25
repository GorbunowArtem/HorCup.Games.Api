namespace HorCup.Games.Requests
{
	public record AddEditGameRequest(string Title,
		int MaxPlayers,
		int MinPlayers,
		string Description);
}