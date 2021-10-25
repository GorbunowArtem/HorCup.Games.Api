using System;

namespace HorCup.Games.Models
{
	public class GameSearchModel
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public int MaxPlayers { get; set; }

		public int MinPlayers { get; set; }
	}
}