using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using CQRSlite.Queries;
using HorCup.Games.Events;
using HorCup.Games.Models;
using HorCup.Games.Queries.SearchGames;
using Nest;
using TupleExtensions;

namespace HorCup.Games.Projections
{
	public class GameSearchProjection : ICancellableEventHandler<GameTitleSet>,
		ICancellableEventHandler<GamePlayersNumberChanged>,
		ICancellableQueryHandler<SearchGamesQuery, (IEnumerable<GameSearchModel> items, long total)>,
		ICancellableEventHandler<GameDeleted>
	{
		private const string GameIndex = "games";
		private readonly ElasticClient _client;

		public GameSearchProjection()
		{
			_client = new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200"))
				.DefaultIndex(GameIndex));
		}

		public Task Handle(GameTitleSet message, CancellationToken token = new()) =>
			_client.IndexDocumentAsync(new GameSearchModel
			{
				Id = message.Id,
				Title = message.Title
			}, token);

		public Task Handle(GamePlayersNumberChanged message, CancellationToken token = new()) =>
			_client.UpdateAsync<GameSearchModel, object>(message.Id,
				g => g.Doc(new
				{
					message.MinPlayers,
					message.MaxPlayers
				}), token);

		public Task Handle(GameDeleted message, CancellationToken token = new()) =>
			_client.DeleteAsync<GameSearchModel>(message.Id, ct: token);

		public async Task<(IEnumerable<GameSearchModel> items, long total)> Handle(
			SearchGamesQuery message,
			CancellationToken token = new())
		{
			var searchRequest = _client.SearchAsync<GameSearchModel>(
				q => q.Query(
					m => m.Bool(
						f => f.Should(
							gm => gm.Term(
								g => g.Title, message.SearchText),
							g => g.Range(
								r => r.GreaterThanOrEquals(message.MinPlayers)
									.Field(game => game.MinPlayers)),
							q => q.Range(
								rang => rang.LessThanOrEquals(message.MaxPlayers)
									.Field(game => game.MaxPlayers))
						)
					)
				)
				, token);

			var totalRequest = _client.CountAsync(new CountRequest(GameIndex), token);

			var (searchResponse, totalResponse) = await (searchRequest, totalRequest).WhenAll();

			return (searchResponse.Documents, totalResponse.Count);
		}
	}
}
