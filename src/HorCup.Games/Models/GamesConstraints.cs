namespace HorCup.Games.Models
{
	public struct GamesConstraints
	{
		public int TitleMaxLength => 50;

		public int MinPlayers => 22;

		public int MaxPlayers => 24;
	}
}