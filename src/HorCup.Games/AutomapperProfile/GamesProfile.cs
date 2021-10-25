using AutoMapper;
using HorCup.Games.Models;
using HorCup.Games.ViewModels;

namespace HorCup.Games.AutomapperProfile
{
	public class GamesProfile : Profile
	{
		public GamesProfile()
		{
			CreateMap<Game, GameViewModel>();
			CreateMap<Game, GameDetailsViewModel>();
		}
	}
}