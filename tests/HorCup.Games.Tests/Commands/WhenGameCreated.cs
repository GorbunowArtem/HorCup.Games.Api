using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CQRSlite.Events;
using FluentAssertions;
using HorCup.Games.CommandHandlers;
using HorCup.Games.Commands;
using HorCup.Games.Events;
using HorCup.Games.Models;
using HorCup.Games.Tests.Factory;
using HorCup.Games.Tests.TestHelpers;
using Xunit;

namespace HorCup.Games.Tests.Commands
{
	public class GameCommandHandlerTests : Specification<Game, GameCommandHandler, CreateGameCommand>
	{
		[Fact]
		public void ShouldRaiseTitleSetEvent()
		{
			var @event = PublishedEvents[0];

			(@event is GameCreated
				{
					Genre: GamesFactory.Genre, MaxPlayers: GamesFactory.MaxPlayers, MinPlayers: GamesFactory.MinPlayers
				}).Should()
				.BeTrue();
		}

		[Fact]
		public void ShouldRaisePlayersNumberChangedEvent()
		{
			var @event = PublishedEvents[1];

			(@event is GameTitleSet { Title: GamesFactory.Title }).Should().BeTrue();
		}

		[Fact]
		public void ShouldRaiseDescriptionChangedEvent()
		{
			var @event = PublishedEvents[2];

			(@event is GameDescriptionChanged { Description: GamesFactory.Description }).Should().BeTrue();
		}

		protected override IEnumerable<IEvent> Given() => Enumerable.Empty<IEvent>();

		protected override CreateGameCommand When() =>
			new Fixture().Build<CreateGameCommand>()
				.With(c => c.Genre, GamesFactory.Genre)
				.With(c => c.Title, GamesFactory.Title)
				.With(c => c.MinPlayers, GamesFactory.MinPlayers)
				.With(c => c.MaxPlayers, GamesFactory.MaxPlayers)
				.With(c => c.Description, GamesFactory.Description)
				.Create();

		protected override GameCommandHandler BuildHandler() =>
			new(Session);
	}
}