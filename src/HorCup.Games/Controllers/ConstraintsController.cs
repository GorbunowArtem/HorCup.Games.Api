using HorCup.Games.Models;
using Microsoft.AspNetCore.Mvc;

namespace HorCup.Games.Controllers
{
	[ApiController]
	[Route("games/constraints")]
	public class ConstraintsController : ControllerBase
	{
		[HttpGet]
		public ActionResult<GamesConstraints> Get() => new(new GamesConstraints());
	}
}