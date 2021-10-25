// using System;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using HorCup.Games.Context;
// using HorCup.Games.Models;
// using HorCup.Infrastructure.Exceptions;
// using Microsoft.EntityFrameworkCore;
//
// namespace HorCup.Games.Services.Games
// {
// 	public class GamesService : IGamesService
// 	{
// 		private readonly IGamesContext _context;
//
// 		public GamesService(IGamesContext context)
// 		{
// 			_context = context;
// 		}
//
// 		public async Task<bool> IsTitleUniqueAsync(
// 			string title,
// 			Guid? id,
// 			CancellationToken cancellationToken)
// 		{
// 			if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
// 			{
// 				return true;
// 			}
//
// 			var query = _context.Games.Where(g => g.Title.ToUpper()
// 				.Equals(title.Trim().ToUpper()));
//
// 			if (id.HasValue)
// 			{
// 				query = query.Where(g => g.Id != id);
// 			}
//
// 			return !await query.AnyAsync(cancellationToken);
// 		}
//
// 		public async Task<Game> TryGetGameAsync(Guid id, CancellationToken cancellationToken)
// 		{
// 			var game = await _context.Games.Where(g => g.Id == id).FirstOrDefaultAsync(cancellationToken);
//
// 			if (game == null)
// 			{
// 				throw new NotFoundException(nameof(Game), id);
// 			}
//
// 			return game;
// 		}
// 	}
// }