using System;
using CQRSlite.Commands;

namespace HorCup.Games.Commands
{
	public record AddGameCommand(
		Guid Id,
		string Title,
		int MinPlayers,
		int MaxPlayers,
		string Description) : ICommand;
}