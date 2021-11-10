using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Queries;
using HorCup.Games.Commands;
using HorCup.Games.Queries;
using HorCup.Games.Requests.CreateGame;
using HorCup.Games.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HorCup.Games.Controllers
{
	[ApiController]
	[Route("games")]
	public class GamesController : ControllerBase
	{
		private readonly ICommandSender _commandSender;
		private readonly IQueryProcessor _queryProcessor;

		public GamesController(
			ICommandSender commandSender,
			IQueryProcessor queryProcessor)
		{
			_commandSender = commandSender;
			_queryProcessor = queryProcessor;
		}

		[HttpGet]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<IActionResult> SearchGames(
			[FromQuery] SearchGamesQuery query,
			CancellationToken token)
		{
			var (items, total) = await _queryProcessor.Query(query, token);

			return Ok(new { Items = items.ToArray(), Total = total });
		}

		[HttpGet("{id:Guid}")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<GameDetailsViewModel>> GetById([FromRoute] Guid id, CancellationToken token)
		{
			var game = await _queryProcessor.Query(new GetGameByIdQuery(id), token);

			return Ok(game);
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		[ProducesResponseType((int)HttpStatusCode.Conflict)]
		public async Task<ActionResult<Guid>> Add(
			[FromBody] CreateGameRequest request)
		{
			var id = Guid.NewGuid();
			var command = new CreateGameCommand(id,
				request.Title,
				request.MinPlayers,
				request.MaxPlayers,
				request.Description,
				request.Genre);

			await _commandSender.Send(command);

			return CreatedAtAction(nameof(Add), new { command.Id }, command.Id.ToString());
		}

		[HttpPatch("{id:Guid}")]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Edit(
			[FromRoute] Guid id,
			[FromBody] CreateGameRequest request)
		{
			var command = new EditGameCommand(
				id,
				request.Title,
				request.MaxPlayers,
				request.MinPlayers,
				request.Description);

			await _commandSender.Send(command);

			return NoContent();
		}

		[HttpDelete("{id:Guid}")]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Delete([FromRoute] Guid id)
		{
			await _commandSender.Send(new DeleteGameCommand(id));

			return NoContent();
		}
	}
}