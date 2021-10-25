using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using HorCup.Games.Models;

namespace HorCup.Games.Commands
{
	public class GameCommandHandler : ICancellableCommandHandler<CreateGameCommand>,
		ICancellableCommandHandler<EditGameCommand>,
		ICancellableCommandHandler<DeleteGameCommand>
	{
		private readonly ISession _session;

		public GameCommandHandler(ISession session)
		{
			_session = session;
		}

		public async Task Handle(CreateGameCommand command, CancellationToken token = new())
		{
			var game = new GameAggregate(command.Id);

			game.SetTitle(command.Title);
			game.SetPlayersCount(command.MinPlayers, command.MaxPlayers);
			game.SetDescription(command.Description);

			await _session.Add(game, token);
			await _session.Commit(token);
		}

		public async Task Handle(EditGameCommand command, CancellationToken token = new())
		{
			var game = await _session.Get<GameAggregate>(command.Id, cancellationToken: token);
			
			game.SetTitle(command.Title);
			game.SetPlayersCount(command.MinPlayers, command.MaxPlayers);
			game.SetDescription(command.Description);

			await _session.Commit(token);
		}

		public async Task Handle(DeleteGameCommand command, CancellationToken token = new())
		{
			var game = await _session.Get<GameAggregate>(command.Id, cancellationToken: token);

			game.Delete();
			
			await _session.Commit(token);
		}
	}
}