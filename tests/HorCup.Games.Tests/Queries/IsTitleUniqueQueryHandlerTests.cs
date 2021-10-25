using System;
using System.Threading.Tasks;
using FluentAssertions;
using HorCup.Games.Queries.IsTitleUnique;
using HorCup.Games.Services.Games;
using HorCup.Games.Tests.Factory;
using Moq;
using NUnit.Framework;

namespace HorCup.Games.Tests.Queries
{
	public class IsTitleUniqueQueryHandlerTests
	{
		private IsTitleUniqueQueryHandler _sut;

		[SetUp]
		public void SetUp()
		{
			var gameServiceMock = new Mock<IGamesService>();
			gameServiceMock.Setup(gs => gs.IsTitleUniqueAsync(GamesFactory.Game1Title, It.IsAny<Guid?>(), default))
				.Returns(Task.FromResult(false));

			gameServiceMock.Setup(gs => gs.IsTitleUniqueAsync(GamesFactory.Game2Title, It.IsAny<Guid?>(), default))
				.Returns(Task.FromResult(true));

			_sut = new IsTitleUniqueQueryHandler(gameServiceMock.Object);
		}

		[TestCase(GamesFactory.Game1Title, false)]
		[TestCase(GamesFactory.Game2Title, true)]
		public async Task Handle_ResultReturned(string title, bool result)
		{
			var isUnique = await _sut.Handle(new IsTitleUniqueQuery(title), default);

			isUnique.Should().Be(result);
		}
	}
}