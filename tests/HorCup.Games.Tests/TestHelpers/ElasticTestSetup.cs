using System;
using System.Collections.Generic;
using TestEnvironment.Docker;

namespace HorCup.Games.Tests.TestHelpers
{
	public class ElasticTestSetup: IDisposable
	{
		private readonly DockerEnvironment _env;

		public ElasticTestSetup()
		{
			_env = new DockerEnvironmentBuilder()
				.AddContainer("elasticsearch", "elastic", "7.14.2",
					new Dictionary<string, string>
					{
						["xpack.security.enabled"] = "false",
						["discovery.type"] = "single-node"
					},
					new Dictionary<ushort, ushort>
					{
						[9200] = 9200,
						[9300] = 9300,
					})
					.Build();

			_env.Up().GetAwaiter().GetResult();
		}

		public void Dispose()
		{
			_env.Down().GetAwaiter().GetResult();
			_env?.Dispose();
		}
	}
}