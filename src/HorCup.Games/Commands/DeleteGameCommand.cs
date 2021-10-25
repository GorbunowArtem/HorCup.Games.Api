using System;
using CQRSlite.Commands;

namespace HorCup.Games.Commands
{
	public record DeleteGameCommand(Guid Id) : ICommand;
}