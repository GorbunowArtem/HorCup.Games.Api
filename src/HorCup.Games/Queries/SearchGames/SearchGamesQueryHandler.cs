// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using AutoMapper;
// using HorCup.Games.Context;
// using HorCup.Games.Models;
// using HorCup.Games.ViewModels;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
//
// namespace HorCup.Games.Queries.SearchGames
// {
// 	public class
// 		SearchGamesQueryHandler : .IRequestHandler<SearchGamesQuery, (IEnumerable<GameViewModel> items, int total)>
// 	{
// 		private readonly IGamesContext _context;
// 		private readonly IMapper _mapper;
// 		private readonly ILogger<SearchGamesQueryHandler> _logger;
//
// 		public SearchGamesQueryHandler(
// 			IGamesContext context,
// 			IMapper mapper,
// 			ILogger<SearchGamesQueryHandler> logger)
// 		{
// 			_context = context;
// 			_mapper = mapper;
// 			_logger = logger;
// 		}
//
// 		public async Task<(IEnumerable<GameViewModel> items, int total)> Handle(
// 			SearchGamesQuery request,
// 			CancellationToken cancellationToken)
// 		{
// 			var query = _context.Games.Where(t => true);
//
// 			query = ApplySearchTextFilter(request, query);
//
// 			query = ApplyPlayersCountFilter(request, query);
//
// 			query = ApplyExceptIdsFilter(request, query);
// 			
// 			var games = await query
// 				.AsNoTracking()
// 				.OrderByDescending(g => g.Added)
// 				.Skip(request.Skip)
// 				.Take(request.Take)
// 				.ToListAsync(cancellationToken);
// 			return (_mapper.Map<IEnumerable<GameViewModel>>(games), await query.CountAsync(CancellationToken.None));
// 		}
//
// 		private static IQueryable<Game> ApplyExceptIdsFilter(SearchGamesQuery request, IQueryable<Game> query)
// 		{
// 			if (request.ExceptIds != null && request.ExceptIds.Any())
// 			{
// 				query = query.Where(g => !request.ExceptIds.Contains(g.Id));
// 			}
//
// 			return query;
// 		}
//
// 		private static IQueryable<Game> ApplyPlayersCountFilter(SearchGamesQuery request, IQueryable<Game> query)
// 		{
// 			if (request.MaxPlayers.HasValue && request.MinPlayers.HasValue)
// 			{
// 				query = query.Where(g => g.MinPlayers >= request.MinPlayers && g.MaxPlayers <= request.MaxPlayers);
// 			}
// 			else
// 			{
// 				if (request.MinPlayers.HasValue)
// 				{
// 					query = query.Where(g => g.MinPlayers >= request.MinPlayers);
// 				}
// 				else if (request.MaxPlayers.HasValue)
// 				{
// 					query = query.Where(g => g.MaxPlayers <= request.MaxPlayers);
// 				}
// 			}
//
// 			return query;
// 		}
//
// 		private static IQueryable<Game> ApplySearchTextFilter(SearchGamesQuery request, IQueryable<Game> query)
// 		{
// 			if (!string.IsNullOrEmpty(request.SearchText))
// 			{
// 				var searchText = request.SearchText.Trim().ToUpper();
//
// 				query = query.Where(g => g.Title.ToUpper().Contains(searchText));
// 			}
//
// 			return query;
// 		}
// 	}
// }