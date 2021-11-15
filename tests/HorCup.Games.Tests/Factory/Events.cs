using AutoFixture;
using HorCup.Games.Events;

namespace HorCup.Games.Tests.Factory
{
	internal class Events
	{
		private readonly GamesFactory _factory;
		private readonly Fixture _fixture;

		public Events(GamesFactory factory)
		{
			_factory = factory;
			_fixture = new Fixture();
		}

		public GameCreated GameCreated() => _fixture.Build<GameCreated>()
			.With(g => g.Id, _factory.Id)
			.With(g => g.Genre, GamesFactory.Genre)
			.With(g => g.MaxPlayers, GamesFactory.MaxPlayers)
			.With(g => g.MinPlayers, GamesFactory.MinPlayers)
			.Create();

		public GameTitleSet GameTitleSet() => _fixture.Build<GameTitleSet>()
			.With(g => g.Id, _factory.Id)
			.With(g => g.Title, GamesFactory.Title)
			.Create();

		public GameDescriptionChanged GameDescriptionChanged() =>
			_fixture.Build<GameDescriptionChanged>()
				.With(g => g.Id, _factory.Id)
				.With(g => g.Description, GamesFactory.Description)
				.Create();
	}
}