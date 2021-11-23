using FluentValidation;
using HorCup.Games.Models;

namespace HorCup.Games.Requests.EditGame
{
	public class EditGameRequestValidator : AbstractValidator<EditGameRequest>
	{
		public EditGameRequestValidator()
		{
			var constraints = new GamesConstraints();

			RuleFor(g => g.Title)
				.NotNull()
				.NotEmpty()
				.MaximumLength(constraints.TitleMaxLength);

			RuleFor(g => g.Description)
				.NotNull()
				.NotEmpty()
				.MaximumLength(500);
		}
	}
}