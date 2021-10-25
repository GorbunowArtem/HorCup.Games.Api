using FluentValidation;
using HorCup.Games.Models;

namespace HorCup.Games.Queries.SearchGames
{
	public class SearchGamesQueryValidator: AbstractValidator<SearchGamesQuery>
	{
		public SearchGamesQueryValidator()
		{
			var constraints = new GamesConstraints();
			
			RuleFor(s => s.MaxPlayers)
				.GreaterThanOrEqualTo(1)
				.LessThanOrEqualTo(constraints.MaxPlayers);
			
			RuleFor(s => s.MinPlayers)
				.GreaterThanOrEqualTo(1)
				.LessThanOrEqualTo(constraints.MinPlayers);

		}
	}
}