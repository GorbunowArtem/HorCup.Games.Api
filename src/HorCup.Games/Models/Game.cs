using System;

namespace HorCup.Games.Models
{
	public class Game
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public int MaxPlayers { get; set; }

		public int MinPlayers { get; set; }

		public DateTime Added { get; set; }
	}
}