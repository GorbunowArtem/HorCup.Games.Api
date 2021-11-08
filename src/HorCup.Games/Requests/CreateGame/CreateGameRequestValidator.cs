using FluentValidation;
using HorCup.Games.Models;

namespace HorCup.Games.Requests.CreateGame
{
	public class CreateGameRequestValidator : AbstractValidator<CreateGameRequest>
	{
		public CreateGameRequestValidator()
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

			RuleFor(g => g.Description)
				.NotNull()
				.NotEmpty()
				.MaximumLength(500);

			RuleFor(g => g.Genre)
				.NotNull()
				.NotEmpty()
				.MaximumLength(50);
		}
	}
}