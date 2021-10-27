using System;

namespace HorCup.Games.Tests.Factory
{
	internal class GamesFactory
	{
		public const string Genre = "Some genre";
		public const string Title = "Some title";
		public const string Description = "Some title";
		public const int MinPlayers = 2;
		public const int MaxPlayers = 6;

		public readonly Guid Id = Guid.NewGuid();
		public Events Events { get; }
		public Queries Queries { get; }

		public GamesFactory()
		{
			Events = new Events(this);
			Queries = new Queries(this);
		}
	}
}