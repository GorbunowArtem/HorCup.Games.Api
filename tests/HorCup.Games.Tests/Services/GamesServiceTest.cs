using System;
using System.Threading.Tasks;
using FluentAssertions;
using HorCup.Games.Models;
using HorCup.Games.Services.Games;
using HorCup.Games.Tests.Factory;
using HorCup.Infrastructure.Exceptions;
using HorCup.Tests.Base;
using NUnit.Framework;

namespace HorCup.Games.Tests.Services
{
	[TestFixture]
	public class GamesServiceTest: TestFixtureBase
	{
		private GamesService _sut;
		private GamesFactory _factory;

		[SetUp]
		public void SetUp()
		{
			_factory = new GamesFactory();
			_sut = new GamesService(Context);
		}

		[TestCase("game 2", 324, false)]
		[TestCase("game 2", 2, true)]
		[TestCase("game 2", null, false)]
		[TestCase("unique title", null, true)]
		[TestCase("", null, true)]
		[TestCase(" ", null, true)]
		[TestCase(null, null, true)]
		public async Task IsTitleUniqueAsync(string title, int id, bool result)
		{
			var isUnique = await _sut.IsTitleUniqueAsync($"   {title}   ", id.Guid(), default);

			isUnique.Should().Be(result);
		}

		[Test]
		public async Task TryGetGameAsync_GameNotExists_ExceptionThrown()
		{
			var id = new Guid();
			
			await _sut.Invoking(action => action.TryGetGameAsync(id, default))
				.Should()
				.ThrowAsync<NotFoundException>()
				.WithMessage($"Entity {nameof(Game)} with key {id.ToString()} was not found");
		}

		[Test]
		public async Task TryGetGameAsync_GameExists_GameReturned()
		{
			var game = await _sut.TryGetGameAsync(_factory.Game2Id, default);

			game.Id.Should().Be(_factory.Game2Id);
		}
	}
}