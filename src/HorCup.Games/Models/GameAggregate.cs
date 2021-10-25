using System;
using CQRSlite.Domain;
using HorCup.Games.Events;

namespace HorCup.Games.Models
{
	public class GameAggregate : AggregateRoot
	{
		public string Title { get; set; }

		public int MinPlayers { get; set; }

		public int MaxPlayers { get; set; }

		public string Description { get; set; }

		public bool Deleted { get; set; }

		public GameAggregate(Guid id)
		{
			Id = id;
		}

		private GameAggregate()
		{
		}

		public void SetTitle(string title)
		{
			if (!string.IsNullOrWhiteSpace(title) &&
			    !string.Equals(title, Title, StringComparison.InvariantCultureIgnoreCase))
			{
				ApplyChange(new GameTitleSet
				{
					Title = title
				});
			}
		}

		public void SetPlayersCount(int minPlayers, int maxPlayers)
		{
			if (minPlayers != MinPlayers
			    || maxPlayers != MaxPlayers)
			{
				ApplyChange(new GamePlayersNumberChanged
				{
					MaxPlayers = maxPlayers,
					MinPlayers = minPlayers,
				});
			}
		}

		public void SetDescription(string description)
		{
			if (!string.Equals(description, Description, StringComparison.InvariantCultureIgnoreCase))
			{
				ApplyChange(new GameDescriptionChanged
				{
					Description = description
				});
			}
		}

		public void Delete()
		{
			if (!Deleted)
			{
				ApplyChange(new GameDeleted());
			}
		}

		private void Apply(GameTitleSet evt)
		{
			Title = evt.Title;
		}

		private void Apply(GamePlayersNumberChanged evt)
		{
			MaxPlayers = evt.MaxPlayers;
			MinPlayers = evt.MinPlayers;
		}


		private void Apply(GameDescriptionChanged evt)
		{
			Description = evt.Description;
		}

		private void Apply(GameDeleted evt)
		{
			Deleted = true;
		}
	}
}