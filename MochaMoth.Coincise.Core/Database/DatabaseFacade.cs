using Microsoft.Extensions.Configuration;

namespace MochaMoth.Coincise.Core.Database
{
	public class DatabaseFacade : IDatabaseFacade
	{
		private readonly IInitializeDatabase _databaseInitializer;

		public DatabaseFacade(IInitializeDatabase databaseInitializer)
		{
			_databaseInitializer = databaseInitializer;
		}

		public void InitializeDatabase(IConfigurationSection configuration) => _databaseInitializer.Initialize(configuration);
	}
}
