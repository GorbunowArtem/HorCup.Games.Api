using System.Collections.Generic;
using CQRSlite.Queries;
using HorCup.Games.Models;

namespace HorCup.Games.Queries
{
	public record SearchGamesQuery(
		string SearchText,
		int? MinPlayers,
		int? MaxPlayers,
		int Take,
		int Skip) : IQuery<(IEnumerable<GameSearchModel> items, long total)>;
}