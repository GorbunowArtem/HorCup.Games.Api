using System;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Commands;
using HorCup.Games.Commands;
using HorCup.Games.EventHandlers;
using HorCup.Games.Events;
using HorCup.Games.Services.Rebuild;
using Microsoft.AspNetCore.Mvc;

namespace HorCup.Games.Controllers
{
	[ApiController]
	[Route("demo")]
	public class DemoController : ControllerBase
	{
		private readonly IProjectionRebuildService _projectionRebuildService;
		private readonly ICommandSender _commandSender;

		public DemoController(IProjectionRebuildService projectionRebuildService, ICommandSender commandSender)
		{
			_projectionRebuildService = projectionRebuildService;
			_commandSender = commandSender;
		}

		[HttpPost]
		[Route("send")]
		public async Task<IActionResult> Execute()
		{
			var c = new GamesEventHandler();

			await c.Handle(new GameTitleSet
			{
				Id = Guid.NewGuid(),
				Title = "From app",
				Version = 1
			});
			
			return Ok();
		}

		[HttpPost]
		[Route("populate")]
		public async Task<IActionResult> CreateTestData()
		{
			var gameCommands = Enumerable.Range(1, 15)
				.Select(
					i => new CreateGameCommand(
						Guid.NewGuid(), $"Title {i}", 2 + i, 4 + i, $"Description {i}", $"Genre {i}"));

			foreach (var createGameCommand in gameCommands)
			{
				await _commandSender.Send(createGameCommand);
			}

			return Ok();
		}
	}
}