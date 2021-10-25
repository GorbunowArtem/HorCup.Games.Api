using System;
using System.Threading;
using System.Threading.Tasks;
using HorCup.Games.Models;

namespace HorCup.Games.Services.Games
{
	public interface IGamesService
	{
		Task<bool> IsTitleUniqueAsync(
			string title,
			Guid? id,
			CancellationToken cancellationToken);
		
		Task<Game> TryGetGameAsync(Guid id, CancellationToken cancellationToken);
	}
}