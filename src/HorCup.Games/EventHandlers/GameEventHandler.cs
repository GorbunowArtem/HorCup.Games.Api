using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Messages;
using HorCup.Games.Events;
using HorCup.Games.External.IntegrationEvents;
using MassTransit;

namespace HorCup.Games.EventHandlers
{
	public class GameEventHandler : ICancellableHandler<GameTitleSet>
	{
		private readonly IPublishEndpoint _publishEndpoint;

		public GameEventHandler(IPublishEndpoint publishEndpoint)
		{
			_publishEndpoint = publishEndpoint;
		}

		public Task Handle(GameTitleSet message, CancellationToken token = new CancellationToken()) =>
			_publishEndpoint.Publish(new GameCreatedIntegrationEvent(message.Id, message.Title), token);
	}
}