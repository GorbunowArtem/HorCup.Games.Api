using HorCup.Games.Queries;

namespace HorCup.Games.Tests.Factory
{
	internal class Queries
	{
		private readonly GamesFactory _factory;

		public Queries(GamesFactory factory)
		{
			_factory = factory;
		}

		public GetGameByIdQuery GetGameByIdQuery() => new(_factory.Id);
	}
}