using System;

namespace HorCup.Games.External.IntegrationEvents
{
	public record GameCreatedIntegrationEvent(Guid Id, string Title);
}