using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using CQRSlite.Caching;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using CQRSlite.Messages;
using CQRSlite.Queries;
using CQRSlite.Routing;
using HorCup.Games.Commands;
using HorCup.Games.EventStore;
using HorCup.Games.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NEventStore;
using NEventStore.Persistence.Sql;
using NEventStore.Persistence.Sql.SqlDialects;
using NEventStore.Serialization.Json;

namespace HorCup.Games.Extensions
{
	public static class EventSourcingExtension
	{
		public static IServiceCollection AddEventSourcing(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddSingleton<Router>(new Router());
			services.AddSingleton<ICommandSender>(y => y.GetService<Router>());
			services.AddSingleton<IEventPublisher>(y => y.GetService<Router>());
			services.AddSingleton<IHandlerRegistrar>(y => y.GetService<Router>());
			services.AddSingleton<IQueryProcessor>(y => y.GetService<Router>());


			CreateDatabaseIfNotExists(
				configuration.GetSection(SqlDbOptions.SqlDb)[nameof(SqlDbOptions.ConnectionString)],
				configuration.GetSection(SqlDbOptions.SqlDb)[nameof(SqlDbOptions.DatabaseName)]);
			
			services.AddSingleton<IStoreEvents>(y => Wireup.Init()
				.WithLoggerFactory(LoggerFactory.Create(logging =>
				{
					logging
						.AddConsole()
						.AddDebug()
						.SetMinimumLevel(LogLevel.Trace);
				}))
				// .UsingInMemoryPersistence()
				.UsingSqlPersistence(new NetStandardConnectionFactory(
					SqlClientFactory.Instance,
					configuration.GetSection(SqlDbOptions.SqlDb)[nameof(SqlDbOptions.ConnectionString)]))
				.WithDialect(new MsSqlDialect())
				.InitializeStorageEngine()
				.UsingJsonSerialization()
				.Compress()
				.Build());

			services.AddScoped<IEventStore, SqlEventStore>();
			services.AddScoped<ICache, MemoryCache>();
			services.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()),
				y.GetService<IEventStore>(), y.GetService<ICache>()));
			services.AddScoped<ISession, Session>();

			//Scan for commandhandlers and eventhandlers
			services.Scan(scan => scan
				.FromAssemblies(typeof(GameCommandHandler).GetTypeInfo().Assembly)
				.AddClasses(classes => classes.Where(x =>
				{
					var allInterfaces = x.GetInterfaces();
					return
						allInterfaces.Any(y =>
							IntrospectionExtensions.GetTypeInfo(y).IsGenericType &&
							IntrospectionExtensions.GetTypeInfo(y).GetGenericTypeDefinition() == typeof(IHandler<>)) ||
						allInterfaces.Any(y =>
							y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() ==
							typeof(ICancellableHandler<>)) ||
						allInterfaces.Any(y =>
							y.GetTypeInfo().IsGenericType &&
							y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IQueryHandler<,>)) ||
						allInterfaces.Any(y =>
							y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() ==
							typeof(ICancellableQueryHandler<,>));
				}))
				.AsSelf()
				.WithTransientLifetime()
			);

			return services;
		}

		private static void CreateDatabaseIfNotExists(string connectionString, string dbName)
		{
			var sb = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = "master" };
			sb.Remove("AttachDBFilename");
			
			SqlCommand cmd = null;
			
			using var connection = new SqlConnection(sb.ConnectionString);
			connection.Open();

			using (cmd = new SqlCommand($"If(db_id(N'{dbName}') IS NULL) CREATE DATABASE [{dbName}]", connection))
			{
				cmd.ExecuteNonQuery();
			}
		}
	}
}