using System;

namespace HorCup.Games.ViewModels
{
	public record GameDetailsViewModel : GameViewModel
	{
		public double? AverageScore { get; set; }

		public DateTime? LastPlayedDate { get; set; }
	}
}