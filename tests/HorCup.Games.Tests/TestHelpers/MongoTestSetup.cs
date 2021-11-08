using System;
using System.Collections.Generic;
using TestEnvironment.Docker;

namespace HorCup.Games.Tests.TestHelpers
{
	public class MongoTestSetup : IDisposable
	{
		private readonly DockerEnvironment _env;

		public MongoTestSetup()
		{
			_env = new DockerEnvironmentBuilder()
				.AddContainer("mongo", "mongo", ports: new Dictionary<ushort, ushort>
				{
					[27017] = 27017
				})
				.Build();

			_env.Up().GetAwaiter().GetResult();
		}

		public void Dispose()
		{
			_env.Down().GetAwaiter().GetResult();
		}
	}
}