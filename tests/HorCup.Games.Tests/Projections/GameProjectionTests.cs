using System.Threading.Tasks;
using FluentAssertions;
using HorCup.Games.Options;
using HorCup.Games.Projections;
using HorCup.Games.Tests.Factory;
using HorCup.Games.Tests.TestHelpers;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace HorCup.Games.Tests.Projections
{
	public class GameProjectionTests: IClassFixture<MongoTestSetup>
	{
		private readonly GamesProjection _sut;
		private readonly GamesFactory _factory;
		private readonly MongoTestSetup _mongoTestSetup;
		
		[Fact]
		public async Task ShouldSetGenre()
		{
			await _sut.Handle(_factory.Events.GameCreated());

			var game = await _sut.Handle(_factory.Queries.GetGameByIdQuery());

			game.Genre.Should().Be(GamesFactory.Genre);
		}

		[Fact]
		public async Task ShouldSetTitle()
		{
			await _sut.Handle(_factory.Events.GameCreated());
			await _sut.Handle(_factory.Events.GameTitleSet());

			var game = await _sut.Handle(_factory.Queries.GetGameByIdQuery());

			game.Title.Should().Be(GamesFactory.Title);
		}

		[Fact]
		public async Task ShouldSetDescription()
		{
			await _sut.Handle(_factory.Events.GameCreated());
			await _sut.Handle(_factory.Events.GameDescriptionChanged());

			var game = await _sut.Handle(_factory.Queries.GetGameByIdQuery());

			game.Description.Should().Be(GamesFactory.Description);
		}

		[Fact]
		public async Task ShouldSetPlayersQuantity()
		{
			await _sut.Handle(_factory.Events.GameCreated());

			var game = await _sut.Handle(_factory.Queries.GetGameByIdQuery());

			(game is
				{
					MaxPlayers: GamesFactory.MaxPlayers,
					MinPlayers: GamesFactory.MinPlayers
				})
				.Should()
				.BeTrue();
		}

		public GameProjectionTests(MongoTestSetup mongoTestSetup)
		{
			_mongoTestSetup = mongoTestSetup;
			var m = new Mock<IOptions<MongoDbOptions>>();
			m.Setup(o => o.Value)
				.Returns(new MongoDbOptions
				{
					ConnectionString = "mongodb://localhost:27017",
					DatabaseName = "Games.Api.Test"
				});

			_factory = new GamesFactory();

			_sut = new GamesProjection(m.Object);
		}
	}
}