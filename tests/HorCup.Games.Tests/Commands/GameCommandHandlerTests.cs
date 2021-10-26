using System.Threading.Tasks;
using AutoFixture;
using CQRSlite.Domain;
using HorCup.Games.Commands;
using Moq;
using Xunit;

namespace HorCup.Games.Tests.Commands
{
	public class GameCommandHandlerTests
	{
		private readonly GameCommandHandler _sut;
		private readonly Fixture _fixture;

		public GameCommandHandlerTests()
		{
			var sessionMock = new Mock<ISession>();

			_fixture = new Fixture();
			
			_sut = new GameCommandHandler(sessionMock.Object);
		}

		[Fact]		
		public async Task Handle_GameCommandCorrect_GameAdded()
		{
			await _sut.Handle(_fixture.Create<AddGameCommand>());
		}
	}
}