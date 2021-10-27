using System;
using CQRSlite.Commands;

namespace HorCup.Games.Commands
{
	public record CreateGameCommand(
		Guid Id,
		string Title,
		int MinPlayers,
		int MaxPlayers,
		string Description,
		string Genre) : ICommand;
}