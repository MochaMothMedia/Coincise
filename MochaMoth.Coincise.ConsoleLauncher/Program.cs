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

			_ = hostBuilder.ConfigureServices((IServiceCollection services) => AddServices(services, configuration));

			IHost host = hostBuilder.Build();

			Task appTask = host.RunAsync();

			IDatabase database = host.Services.GetService<IDatabase>()!;
			IAPIFacade webAPI = host.Services.GetService<IAPIFacade>()!;

			webAPI.RunAPI();

			appTask.Wait();
		}

		private static void AddServices(IServiceCollection services, IConfigurationSection configuration)
		{
			AddLogging(services);
			AddDatabase(services, configuration);
			AddWebAPI(services);
		}

		private static void AddLogging(IServiceCollection services)
		{
			_ = services.AddScoped<ILogInfo, InfoLogger>();
			_ = services.AddScoped<ILogWarning, WarningLogger>();
			_ = services.AddScoped<ILogError, ErrorLogger>();

			_ = services.AddScoped<ILog, Logger>();
		}

		private static void AddDatabase(IServiceCollection services, IConfigurationSection configuration)
		{
			MongoDatabaseInitializer.Initialize(configuration.GetRequiredSection("Databases").GetRequiredSection("MongoDB"), services);

			_ = services.AddScoped<IDatabase, Database>();
		}

		private static void AddWebAPI(IServiceCollection services)
		{
			_ = services.AddScoped<IAPIRunner, ASPNETWebAPIRunner>();

			_ = services.AddScoped<IAPIFacade, APIFacade>();
		}
	}
}