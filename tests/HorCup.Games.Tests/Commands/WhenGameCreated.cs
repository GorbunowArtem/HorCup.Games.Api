using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CQRSlite.Events;
using FluentAssertions;
using HorCup.Games.CommandHandlers;
using HorCup.Games.Commands;
using HorCup.Games.Events;
using HorCup.Games.Models;
using HorCup.Games.Tests.TestHelpers;
using Xunit;

namespace HorCup.Games.Tests.Commands
{
	public class GameCommandHandlerTests : Specification<Game, GameCommandHandler, CreateGameCommand>
	{
		private const string Title = "Sometitle";
		private const string Description = "SomeDescription";
		private const int MinPlayers = 4;
		private const int MaxPlayers = 6;

		[Fact]
		public void ShouldRaiseTitleSetEvent()
		{
			(PublishedEvents.First() is GameTitleSet { Title: Title }).Should().BeTrue();
		}

		[Fact]
		public void ShouldRaisePlayersNumberChangedEvent()
		{
			var @event = PublishedEvents[1];

			(@event is GamePlayersNumberChanged { MaxPlayers: MaxPlayers, MinPlayers: MinPlayers }).Should().BeTrue();
		}

		[Fact]
		public void ShouldRaiseDescriptionChangedEvent()
		{
			var @event = PublishedEvents[2];

			(@event is GameDescriptionChanged { Description: Description }).Should().BeTrue();
		}

		protected override IEnumerable<IEvent> Given() => Enumerable.Empty<IEvent>();

		protected override CreateGameCommand When() =>
			new Fixture().Build<CreateGameCommand>()
				.With(c => c.Title, Title)
				.With(c => c.MinPlayers, MinPlayers)
				.With(c => c.MaxPlayers, MaxPlayers)
				.With(c => c.Description, Description)
				.Create();

		protected override GameCommandHandler BuildHandler() =>
			new(Session);
	}
}