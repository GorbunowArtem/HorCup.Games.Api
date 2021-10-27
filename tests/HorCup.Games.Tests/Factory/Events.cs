using HorCup.Games.Events;

namespace HorCup.Games.Tests.Factory
{
	internal class Events
	{
		private readonly GamesFactory _factory;

		public GameCreated GameCreated() => new()
		{
			Id = _factory.Id
		};

		public Events(GamesFactory factory)
		{
			_factory = factory;
		}

		public GameTitleSet GameTitleSet() => new()
		{
			Id = _factory.Id,
			Title = GamesFactory.Title
		};

		public GameDescriptionChanged GameDescriptionChanged() => new()
		{
			Id = _factory.Id,
			Description = GamesFactory.Description
		};

		public GamePlayersNumberChanged GamePlayersNumberChanged() => new()
		{
			Id = _factory.Id,
			MaxPlayers = GamesFactory.MaxPlayers,
			MinPlayers = GamesFactory.MinPlayers,
		};
	}
}