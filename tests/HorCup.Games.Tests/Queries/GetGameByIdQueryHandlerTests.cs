using System.Threading.Tasks;
using FluentAssertions;
using HorCup.Games.Queries;
using HorCup.Games.Services.Games;
using HorCup.Games.Tests.Factory;
using HorCup.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace HorCup.Games.Tests.Queries
{
	public class GetGameByIdQueryHandlerTests : TestFixtureBase
	{
		private readonly GamesFactory _factory = new();
		private GetGameByIdQueryHandler _sut;

		[SetUp]
		public void SetUp()
		{
			var gameServiceMock = new Mock<IGamesService>();
			gameServiceMock.Setup(gs => gs.TryGetGameAsync(_factory.NotExistingGameId, default))
				.Throws<NotFoundException>();

			_sut = new GetGameByIdQueryHandler(Context, NullLogger<GetGameByIdQueryHandler>.Instance,
				Mapper,
				gameServiceMock.Object);
		}

		[Test]
		public async Task Handle_GameDoesNotExists_ExceptionThrown()
		{
			await _sut.Invoking(handler =>
					handler.Handle(new GetGameByIdQuery(_factory.NotExistingGameId), default))
				.Should()
				.ThrowAsync<NotFoundException>();
		}

		[Test]
		public async Task Handle_GameHasNoStatistic_GameWithDefaultStatisticReturned()
		{
			var details = await _sut.Handle(new GetGameByIdQuery(_factory.Game4Id), default);

			details.AverageScore.Should().BeNull();
			details.TimesPlayed.Should().Be(0);
			details.LastPlayedDate.Should().BeNull();
			details.Id.Should().Be(_factory.Game4Id);
			details.Title.Should().Be(GamesFactory.Game4Title);
			details.MaxPlayers.Should().Be(GamesFactory.Game4MaxPlayers);
			details.MaxPlayers.Should().Be(GamesFactory.Game4MaxPlayers);
		}
	}
}