using System;

namespace HorCup.Games.Models
{
	public class GameDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }

		public int MinPlayers { get; set; }

		public int MaxPlayers { get; set; }

		public string Description { get; set; }
	}
}