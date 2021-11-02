using FluentValidation.AspNetCore;
using HorCup.Games.Extensions;
using HorCup.Games.Options;
using HorCup.Games.Requests;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace HorCup.Games
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<SqlDbOptions>(Configuration.GetSection(SqlDbOptions.SqlDb));
			services.Configure<MongoDbOptions>(Configuration.GetSection(MongoDbOptions.MongoDb));
			services.Configure<EsOptions>(Configuration.GetSection(EsOptions.Elasticsearch));

			services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "HorCup", Version = "v1" }); });

			services.AddEventSourcing(Configuration);

			services.AddHealthChecks();

			services.AddControllers()
				.AddFluentValidation(v => v.RegisterValidatorsFromAssemblyContaining<CreateGameCommandValidator>());

			services.AddMassTransit(t =>
				// t.UsingRabbitMq((context, configuration) => { configuration.Host("rabbitmq"); }));
				t.UsingRabbitMq());

			services.AddMassTransitHostedService();
		}

		public void Configure(
			IApplicationBuilder app,
			IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors("AllowAll");

			app.UseSwagger();

			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HorCup.Games v1"));

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHealthChecks("/health");
			});
		}
	}
}