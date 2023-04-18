using MochaMoth.Coincise.ConsoleLauncher.Logging;
using MochaMoth.Coincise.Core.Database;
using MochaMoth.Coincise.Core.Logging;
using MochaMoth.Coincise.Core.WebAPI;
using MochaMoth.Coincise.WebAPI;

namespace MochaMoth.Coincise.ConsoleLauncher
{
	public class Program
	{
		public static void Main()
		{
			IServiceCollection services = new ServiceCollection();

			AddLogging(services);
			AddDatabase(services);
			AddWebAPI(services);

			IServiceProvider provider = services.BuildServiceProvider();

			IAPIFacade webAPIFacade = provider.GetService<IAPIFacade>()!;

			webAPIFacade.RunAPI();
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
			_ = services.AddScoped<IDatabaseFacade, DatabaseFacade>();
		}

		private static void AddWebAPI(IServiceCollection services)
		{
			_ = services.AddScoped<IAPIRunner, ASPNETWebAPIRunner>();

			_ = services.AddScoped<IAPIFacade, APIFacade>();
		}
	}
}