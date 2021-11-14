using System;
using CQRSlite.Commands;

namespace HorCup.Games.Commands
{
	public record EditGameCommand(
		Guid Id,
		string Title,
		string Description) : ICommand;
}