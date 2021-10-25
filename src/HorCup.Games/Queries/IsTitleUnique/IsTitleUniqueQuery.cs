using System;

namespace HorCup.Games.Queries.IsTitleUnique
{
	public record IsTitleUniqueQuery(string Title, Guid? Id = null);
}