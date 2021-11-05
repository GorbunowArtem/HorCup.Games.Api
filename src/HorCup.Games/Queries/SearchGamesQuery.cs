using System.Collections.Generic;
using CQRSlite.Queries;
using HorCup.Games.Models;

namespace HorCup.Games.Queries
{
	public record SearchGamesQuery : IQuery<(IEnumerable<GameSearchModel> items, long total)>
	{
		public string SearchText { get; set; }

		public int? MinPlayers { get; set; }

		public int? MaxPlayers { get; set; }

		public int Take { get; set; } = 10;

		public int Skip { get; set; }
	}
}