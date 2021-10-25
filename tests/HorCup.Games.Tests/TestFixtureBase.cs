using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HorCup.Games.AutomapperProfile;
using HorCup.Games.Context;
using HorCup.Tests.Base;

namespace HorCup.Games.Tests
{
	public abstract class TestFixtureBase
	{
		protected readonly GamesContext Context;

		protected TestFixtureBase()
		{
			Context = GamesContextFactory.Create();

			var configProvider = new MapperConfiguration(cfg => { cfg.AddProfile<GamesProfile>(); });

			Mapper = configProvider.CreateMapper();
		}

		public void Dispose()
		{
			GamesContextFactory.Destroy(Context);
		}

		protected IMapper Mapper { get; }

		protected static IEnumerable<Guid> ToGuidList(IEnumerable<int> ids) =>
			ids.Select(i => i.Guid()).ToArray();
	}
}