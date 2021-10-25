using System;
using CQRSlite.Queries;
using HorCup.Games.Models;

namespace HorCup.Games.Queries
{
	public record GetGameByIdQuery(Guid Id) : IQuery<GameDto>;
}