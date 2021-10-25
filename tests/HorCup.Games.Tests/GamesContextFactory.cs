using System;
using HorCup.Games.Context;
using HorCup.Games.Tests.Factory;
using Microsoft.EntityFrameworkCore;

namespace HorCup.Games.Tests
{
	public static class GamesContextFactory
	{
		public static GamesContext Create()
		{
			var options = new DbContextOptionsBuilder<GamesContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			var context = new GamesContext(options);
			context.Database.EnsureCreated();

			context.Games.AddRange(new GamesFactory().Games);

			context.SaveChanges();

			return context;
		}

		public static void Destroy(GamesContext context)
		{
			context.Database.EnsureDeleted();

			context.Dispose();
		}

	}
}