using MochaMoth.Coincise.ConsoleLauncher.Logging;
using MochaMoth.Coincise.Core.Database;
using MochaMoth.Coincise.Core.Logging;
using MochaMoth.Coincise.Core.WebAPI;
using MochaMoth.Coincise.MongoDatabase.Database;
using MochaMoth.Coincise.WebAPI;

namespace MochaMoth.Coincise.ConsoleLauncher
{
	public class Program
	{
		public static void Main(string[]? args)
		{
			IConfigurationSection configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				.Build()
				.GetRequiredSection("Settings");

			IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);

			_ = hostBuilder.ConfigureServices(AddServices);

			IHost host = hostBuilder.Build();

			Task appTask = host.RunAsync();

			IDatabaseFacade database = host.Services.GetService<IDatabaseFacade>()!;
			IAPIFacade webAPI = host.Services.GetService<IAPIFacade>()!;

			database.InitializeDatabase(configuration.GetRequiredSection("Databases").GetRequiredSection("MongoDB"));
			webAPI.RunAPI();

			appTask.Wait();
		}

		private static void AddServices(IServiceCollection services)
		{
			AddLogging(services);
			AddDatabase(services);
			AddWebAPI(services);
		}

		private static void AddLogging(IServiceCollection services)
		{
			_ = services.AddScoped<ILogInfo, InfoLogger>();
			_ = services.AddScoped<ILogWarning, WarningLogger>();
			_ = services.AddScoped<ILogError, ErrorLogger>();

			_ = services.AddScoped<ILogFacade, LogFacade>();
		}

		private static void AddDatabase(IServiceCollection services)
		{
			_ = services.AddScoped<IInitializeDatabase, MongoDatabaseInitializer>();

			_ = services.AddScoped<IDatabaseFacade, DatabaseFacade>();
		}

		private static void AddWebAPI(IServiceCollection services)
		{
			_ = services.AddScoped<IAPIRunner, ASPNETWebAPIRunner>();

			_ = services.AddScoped<IAPIFacade, APIFacade>();
		}
	}
}