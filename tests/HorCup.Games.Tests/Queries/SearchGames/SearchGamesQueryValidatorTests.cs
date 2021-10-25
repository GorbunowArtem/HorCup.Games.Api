using FluentValidation.TestHelper;
using HorCup.Games.Queries.SearchGames;
using NUnit.Framework;

namespace HorCup.Games.Tests.Queries.SearchGames
{
	[TestFixture]
	public class SearchGamesQueryValidatorTests
	{
		private SearchGamesQueryValidator _sut;

		[SetUp]
		public void SetUp()
		{
			_sut = new SearchGamesQueryValidator();
		}
		
		[TestCase(25)]
		[TestCase(-1)]
		[TestCase(0)]
		public void SearchGamesQueryValidator_MaxPlayersIsInvalid_ValidationErrorThrown(int? maxPlayers)
		{
			var model = new SearchGamesQuery
			{
				MaxPlayers = maxPlayers
			};

			var result = _sut.TestValidate(model);

			result.ShouldHaveValidationErrorFor(s => s.MaxPlayers);
		}
		
		[TestCase(24)]
		[TestCase(null)]
		[TestCase(1)]
		[TestCase(5)]
		public void SearchGamesQueryValidator_MaxPlayersIsValid_ValidationPassed(int? maxPlayers)
		{
			var model = new SearchGamesQuery
			{
				MaxPlayers = maxPlayers
			};

			var result = _sut.TestValidate(model);

			result.ShouldNotHaveValidationErrorFor(s => s.MaxPlayers);
		}
		
		[TestCase(23)]
		[TestCase(-1)]
		[TestCase(0)]
		public void SearchGamesQueryValidator_MinPlayersIsInvalid_ValidationErrorThrown(int? minPlayers)
		{
			var model = new SearchGamesQuery
			{
				MinPlayers = minPlayers
			};

			var result = _sut.TestValidate(model);

			result.ShouldHaveValidationErrorFor(s => s.MinPlayers);
		}
		
		[TestCase(22)]
		[TestCase(null)]
		[TestCase(1)]
		[TestCase(5)]
		public void SearchGamesQueryValidator_MinPlayersIsValid_ValidationPassed(int? minPlayers)
		{
			var model = new SearchGamesQuery
			{
				MinPlayers = minPlayers
			};

			var result = _sut.TestValidate(model);

			result.ShouldNotHaveValidationErrorFor(s => s.MaxPlayers);
		}
	}
}