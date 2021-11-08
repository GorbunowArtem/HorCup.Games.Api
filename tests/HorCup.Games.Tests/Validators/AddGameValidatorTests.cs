using AutoFixture;
using FluentValidation.TestHelper;
using HorCup.Games.Commands;
using HorCup.Games.Requests.CreateGame;
using Xunit;

namespace HorCup.Games.Tests.Validators
{
	public class AddGameValidatorTests
	{
		private readonly CreateGameRequestValidator _validator;
		private readonly Fixture _fixture;

		public AddGameValidatorTests()
		{
			_validator = new CreateGameRequestValidator();
			_fixture = new Fixture();
		}

		[Theory]
		[InlineData(null ,"'Title' must not be empty.")]
		[InlineData("" ,"'Title' must not be empty.")]
		[InlineData("More symbols than described in GamesConstraints cla", "The length of 'Title' must be 50 characters or fewer. You entered 51 characters.")]
		public void AddGameCommandValidator_TitleInvalid_ValidationErrorThrown(string title, string errorMessage)
		{
			var model = _fixture.Build<CreateGameRequest>()
				.With(t => t.Title, title)
				.Create();
			
			var result = _validator.TestValidate(model);
			
			result.ShouldHaveValidationErrorFor(g => g.Title)
				.WithErrorMessage(errorMessage);
		}

		[Theory]
		[InlineData("ValidTitle")]
		[InlineData("T")]
		[InlineData("Title with maximum available length 12345678910112")]
		public void AddGameCommandValidator_TitleValid_ValidationPassed(string title)
		{
			var model = _fixture.Build<CreateGameRequest>()
				.With(t => t.Title, title)
				.Create();

			var result = _validator.TestValidate(model);

			result.ShouldNotHaveValidationErrorFor(g => g.Title);
		}

		[Theory]
		[InlineData(0, 0, "'Max Players' must be greater than or equal to '1'.")]
		[InlineData(25, 0, "'Max Players' must be less than or equal to '24'.")]
		[InlineData(1, 2, "'Max Players' must be greater than or equal to '2'.")]
		public void AddGameValidator_MaxPlayersCountInvalid_ValidationErrorThrown(int maxPlayers, int minPlayers, string errorMessage)
		{
			var model = _fixture.Build<CreateGameRequest>()
				.With(t => t.MinPlayers, minPlayers)
				.With(t => t.MaxPlayers, maxPlayers)
				.Create();

			var result = _validator.TestValidate(model);

			result.ShouldHaveValidationErrorFor(g => g.MaxPlayers)
				.WithErrorMessage(errorMessage);
		}

		[Theory]
		[InlineData(1, 1)]
		[InlineData(24, 22)]
		[InlineData(10, 10)]
		public void AddGameValidator_MaxPlayersCountValid_ValidationPassed(int maxPlayers, int minPlayers)
		{
			var model = _fixture.Build<CreateGameRequest>()
				.With(t => t.MinPlayers, minPlayers)
				.With(t => t.MaxPlayers, maxPlayers)
				.Create();

			var result = _validator.TestValidate(model);

			result.ShouldNotHaveValidationErrorFor(g => g.MaxPlayers);
		}
		
		[Theory]
		[InlineData(0, 0, "'Min Players' must be greater than or equal to '1'.")]
		[InlineData(23, 0, "'Min Players' must be less than or equal to '22'.")]
		[InlineData(2, 1, "'Min Players' must be less than or equal to '1'.")]
		public void AddGameValidator_MinPlayersCountInvalid_ValidationErrorThrown(int minPlayers, int maxPlayers, string errorMessage)
		{
			var model = _fixture.Build<CreateGameRequest>()
				.With(t => t.MinPlayers, minPlayers)
				.With(t => t.MaxPlayers, maxPlayers)
				.Create();


			var result = _validator.TestValidate(model);

			result.ShouldHaveValidationErrorFor(g => g.MinPlayers)
				.WithErrorMessage(errorMessage);
		}

		[Theory]
		[InlineData(1, 1)]
		[InlineData(22, 22)]
		[InlineData(11, 12)]
		public void AddGameValidator_MinPlayersCountValid_ValidationPassed(int minPlayers, int maxPlayers)
		{
			var model = _fixture.Build<CreateGameRequest>()
				.With(t => t.MinPlayers, minPlayers)
				.With(t => t.MaxPlayers, maxPlayers)
				.Create();


			var result = _validator.TestValidate(model);

			result.ShouldNotHaveValidationErrorFor(g => g.MinPlayers);
		}
	}
}