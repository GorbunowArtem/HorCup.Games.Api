using System;

namespace HorCup.Games.ViewModels
{
	public record GameViewModel
	{
		public Guid Id { get; set; }

		public string Title { get; set; }
		
		public int MaxPlayers { get; set; }
		
		public int MinPlayers { get; set; }

		public int TimesPlayed { get; set; }
	}
}