using System;
using System.IO;
using CQRSlite.Routing;
using HorCup.Games.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HorCup.Games
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
					webBuilder.UseStartup<Startup>();
				})
				.UseServiceProviderFactory(new RouteRegistrarProviderFactory());

		public class RouteRegistrarProviderFactory : IServiceProviderFactory<IServiceCollection>
		{
			public IServiceCollection CreateBuilder(IServiceCollection services)
			{
				return services;
			}

			public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
			{
				var serviceProvider = containerBuilder.BuildServiceProvider();
				var registrar = new RouteRegistrar(new Provider(serviceProvider));
				registrar.RegisterInAssemblyOf(typeof(GameCommandHandler));
				return serviceProvider;
			}
		}

		public class Provider : IServiceProvider
		{
			private readonly ServiceProvider _serviceProvider;
			private readonly IHttpContextAccessor _contextAccessor;

			public Provider(ServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider;
				_contextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
			}

			public object GetService(Type serviceType)
			{
				return _contextAccessor?.HttpContext?.RequestServices.GetService(serviceType) ??
				       _serviceProvider.GetService(serviceType);
			}
		}

	}
}