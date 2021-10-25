using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HorCup.Games.Commands.DeleteGame;
using HorCup.Games.Models;
using HorCup.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace HorCup.Games.Tests.Commands
{
	public class DeleteGameCommandHandlerTests: TestFixtureBase
	{
		private DeleteGameCommandHandler _sut;

		[SetUp]
		public void SetUp()
		{
			_sut = new DeleteGameCommandHandler(Context, NullLogger<DeleteGameCommandHandler>.Instance);
		}

		[Test]
		public async Task Handle_GameNotExists_ExceptionThrown()
		{
			var id = new Guid();
			
			await _sut.Invoking(handler => handler.Handle(new DeleteGameCommand(id), CancellationToken.None))
				.Should().ThrowAsync<NotFoundException>()
				.WithMessage($"Entity {nameof(Game)} with key {id.ToString()} was not found");
		}

		[Test]
		public async Task Handle_GameExists_GameDeleted()
		{
			var game = Context.Games.First();

			await _sut.Handle(new DeleteGameCommand(game.Id), CancellationToken.None);

			var actual = Context.Games.FirstOrDefault(g => g.Id == game.Id);

			actual.Should().BeNull();
		}
		
	}
}