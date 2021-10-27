using FluentValidation;
using HorCup.Games.Models;

namespace HorCup.Games.Commands
{
	public class AddGameCommandValidator: AbstractValidator<CreateGameCommand>
	{
		public AddGameCommandValidator()
		{
			var constraints = new GamesConstraints();
			
			RuleFor(g => g.Title)
				.NotNull()
				.NotEmpty()
				.MaximumLength(constraints.TitleMaxLength);

			RuleFor(g => g.MaxPlayers)
				.GreaterThanOrEqualTo(1)
				.LessThanOrEqualTo(constraints.MaxPlayers)
				.GreaterThanOrEqualTo(p => p.MinPlayers);
			
			RuleFor(g => g.MinPlayers)
				.GreaterThanOrEqualTo(1)
				.LessThanOrEqualTo(constraints.MinPlayers)
				.LessThanOrEqualTo(p => p.MaxPlayers);
		}
	}
}