using System;
using FluentValidation;
using HorCup.Games.Models;

namespace HorCup.Games.Requests.EditGame
{
	public class EditGameRequestValidator: AbstractValidator<EditGameRequest>
	{
		public EditGameRequestValidator()
		{
			var constraints = new GamesConstraints();

			RuleFor(g => g.Id)
				.NotEqual(Guid.Empty);
			
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