using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Queries;
using HorCup.Games.Commands;
using HorCup.Games.Queries;
using HorCup.Games.Queries.SearchGames;
using HorCup.Games.Requests;
using HorCup.Games.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HorCup.Games.Controllers
{
	[ExcludeFromCodeCoverage]
	[ApiController]
	[Route("games")]
	public class GamesController : Controller
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
			[FromQuery] SearchGamesQuery query)
		{
			var result = await _queryProcessor.Query(query);

			return Ok(result);
		}

		[HttpGet("{id:Guid}")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<GameDetailsViewModel>> GetById([FromRoute] Guid id)
		{
			var game = await _queryProcessor.Query(new GetGameByIdQuery(id));

			return Ok(game);
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		[ProducesResponseType((int)HttpStatusCode.Conflict)]
		public async Task<ActionResult<Guid>> Add(
			[FromBody] AddEditGameRequest request,
			CancellationToken cancellationToken)
		{
			var id = Guid.NewGuid();
			var command = new CreateGameCommand(id,
				request.Title,
				request.MinPlayers,
				request.MaxPlayers,
				request.Description);

			await _commandSender.Send(command, cancellationToken);

			return CreatedAtAction(nameof(Add), new { command.Id }, command.Id.ToString());
		}

		[HttpPatch("{id:Guid}")]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Edit(
			[FromRoute] Guid id,
			[FromBody] AddEditGameRequest request,
			CancellationToken token)
		{
			var command = new EditGameCommand(
				id,
				request.Title,
				request.MaxPlayers,
				request.MinPlayers,
				request.Description);

			await _commandSender.Send(command, token);

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
		//
		// [HttpHead]
		// [ProducesResponseType((int) HttpStatusCode.OK)]
		// [ProducesResponseType((int) HttpStatusCode.Conflict)]
		// public async Task<IActionResult> IsTitleUnique(string title, Guid? id)
		// {
		// 	var isUnique = await _sender.Send(new IsTitleUniqueQuery(title, id));
		//
		// 	if (!isUnique)
		// 	{
		// 		return Conflict("Title is not unique.");
		// 	}
		//
		// 	return Ok();
		// }
	}
}