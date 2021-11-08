using System;

namespace HorCup.Games.Requests.EditGame
{
	public record EditGameRequest(
		Guid Id,
		string Title,
		int MaxPlayers,
		int MinPlayers,
		string Description,
		string Genre);
}