using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using CQRSlite.Queries;
using HorCup.Games.Events;
using HorCup.Games.Models;
using HorCup.Games.Options;
using HorCup.Games.Queries;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HorCup.Games.Projections
{
	public class GamesProjection : ICancellableEventHandler<GameTitleSet>,
		ICancellableEventHandler<GamePlayersNumberChanged>,
		ICancellableEventHandler<GameDescriptionChanged>,
		ICancellableQueryHandler<GetGameByIdQuery, GameDto>,
		ICancellableEventHandler<GameDeleted>
	{
		private readonly IMongoCollection<GameDto> _games;

		public GamesProjection(IOptions<MongoDbOptions> options)
		{
			var client = new MongoClient(options.Value.ConnectionString).GetDatabase("GamesApiRead");

			_games = client.GetCollection<GameDto>("Games");
		}

		public Task Handle(GameTitleSet message, CancellationToken token = new()) =>
			_games.UpdateOneAsync(Builders<GameDto>.Filter.Eq(g => g.Id, message.Id),
				Builders<GameDto>.Update
					.Set(g => g.Id, message.Id)
					.Set(g => g.Title, message.Title), new UpdateOptions
				{
					IsUpsert = true
				}, token);

		public Task<GameDto> Handle(GetGameByIdQuery message, CancellationToken token = new()) =>
			_games.Find(Builders<GameDto>.Filter.Eq(g => g.Id, message.Id)).FirstOrDefaultAsync(token);

		public Task Handle(GamePlayersNumberChanged message, CancellationToken token = new()) =>
			_games.UpdateOneAsync(Builders<GameDto>.Filter.Eq(g => g.Id, message.Id),
				Builders<GameDto>.Update.Set(g => g.MaxPlayers, message.MaxPlayers)
					.Set(g => g.MinPlayers, message.MinPlayers), cancellationToken: token);

		public Task Handle(GameDescriptionChanged message, CancellationToken token = new()) =>
			_games.UpdateOneAsync(Builders<GameDto>.Filter.Eq(g => g.Id, message.Id),
				Builders<GameDto>.Update.Set(g => g.Description, message.Description), cancellationToken: token);

		public Task Handle(GameDeleted message, CancellationToken token = new()) =>
			_games.DeleteOneAsync(Builders<GameDto>.Filter.Eq(g => g.Id, message.Id), token);
	}
}