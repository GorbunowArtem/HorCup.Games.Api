using System;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Commands;
using HorCup.Games.Commands;
using HorCup.Games.Services.Rebuild;
using Microsoft.AspNetCore.Mvc;

namespace HorCup.Games.Controllers
{
	[ApiController]
	[Route("rebuild")]
	public class Rebuild : ControllerBase
	{
		private readonly IProjectionRebuildService _projectionRebuildService;
		private readonly ICommandSender _commandSender;

		public Rebuild(IProjectionRebuildService projectionRebuildService, ICommandSender commandSender)
		{
			_projectionRebuildService = projectionRebuildService;
			_commandSender = commandSender;
		}

		[HttpPost]
		public IActionResult Execute()
		{
			_projectionRebuildService.Execute();

			return Ok();
		}

		[HttpPost]
		[Route("populate")]
		public async Task CreateTestData()
		{
			var gameCommands = Enumerable.Range(1, 15)
				.Select(
					i => new CreateGameCommand(
						Guid.NewGuid(), $"Title {i}", 2 + i, 4 + i, $"Description {i}", $"Genre {i}"));

			foreach (var createGameCommand in gameCommands)
			{
				await _commandSender.Send(createGameCommand);
			}
		}
	}
}