using AutoFixture;
using FluentValidation.TestHelper;
using HorCup.Games.Commands;
using NUnit.Framework;

namespace HorCup.Games.Tests.Commands.AddGame
{
	[TestFixture]
	public class AddGameValidatorTests
	{
		private AddGameCommandValidator _validator;
		private Fixture _fixture;

		[SetUp]
		public void SetUp()
		{
			_validator = new AddGameCommandValidator();
			_fixture = new Fixture();
		}

		[TestCase(null ,"'Title' must not be empty.")]
		[TestCase("" ,"'Title' must not be empty.")]
		[TestCase("More symbols than described in GamesConstraints cla", "The length of 'Title' must be 50 characters or fewer. You entered 51 characters.")]
		public void AddGameCommandValidator_TitleInvalid_ValidationErrorThrown(string title, string errorMessage)
		{
			var model = _fixture.Build<AddGameCommand>()
				.With(t => t.Title, title)
				.Create();
			
			var result = _validator.TestValidate(model);
			
			result.ShouldHaveValidationErrorFor(g => g.Title)
				.WithErrorMessage(errorMessage);
		}

		[TestCase("ValidTitle")]
		[TestCase("T")]
		[TestCase("Title with maximum available length 12345678910112")]
		public void AddGameCommandValidator_TitleValid_ValidationPassed(string title)
		{
			var model = _fixture.Build<AddGameCommand>()
				.With(t => t.Title, title)
				.Create();

			var result = _validator.TestValidate(model);

			result.ShouldNotHaveValidationErrorFor(g => g.Title);
		}

		[TestCase(0, 0, "'Max Players' must be greater than or equal to '1'.")]
		[TestCase(25, 0, "'Max Players' must be less than or equal to '24'.")]
		[TestCase(1, 2, "'Max Players' must be greater than or equal to '2'.")]
		public void AddGameValidator_MaxPlayersCountInvalid_ValidationErrorThrown(int maxPlayers, int minPlayers, string errorMessage)
		{
			var model = _fixture.Build<AddGameCommand>()
				.With(t => t.MinPlayers, minPlayers)
				.With(t => t.MaxPlayers, maxPlayers)
				.Create();

			var result = _validator.TestValidate(model);

			result.ShouldHaveValidationErrorFor(g => g.MaxPlayers)
				.WithErrorMessage(errorMessage);
		}

		[TestCase(1, 1)]
		[TestCase(24, 22)]
		[TestCase(10, 10)]
		public void AddGameValidator_MaxPlayersCountValid_ValidationPassed(int maxPlayers, int minPlayers)
		{
			var model = _fixture.Build<AddGameCommand>()
				.With(t => t.MinPlayers, minPlayers)
				.With(t => t.MaxPlayers, maxPlayers)
				.Create();

			var result = _validator.TestValidate(model);

			result.ShouldNotHaveValidationErrorFor(g => g.MaxPlayers);
		}
		
		[TestCase(0, 0, "'Min Players' must be greater than or equal to '1'.")]
		[TestCase(23, 0, "'Min Players' must be less than or equal to '22'.")]
		[TestCase(2, 1, "'Min Players' must be less than or equal to '1'.")]
		public void AddGameValidator_MinPlayersCountInvalid_ValidationErrorThrown(int minPlayers, int maxPlayers, string errorMessage)
		{
			var model = _fixture.Build<AddGameCommand>()
				.With(t => t.MinPlayers, minPlayers)
				.With(t => t.MaxPlayers, maxPlayers)
				.Create();


			var result = _validator.TestValidate(model);

			result.ShouldHaveValidationErrorFor(g => g.MinPlayers)
				.WithErrorMessage(errorMessage);
		}

		[TestCase(1, 1)]
		[TestCase(22, 22)]
		[TestCase(11, 12)]
		public void AddGameValidator_MinPlayersCountValid_ValidationPassed(int minPlayers, int maxPlayers)
		{
			var model = _fixture.Build<AddGameCommand>()
				.With(t => t.MinPlayers, minPlayers)
				.With(t => t.MaxPlayers, maxPlayers)
				.Create();


			var result = _validator.TestValidate(model);

			result.ShouldNotHaveValidationErrorFor(g => g.MinPlayers);
		}
	}
}