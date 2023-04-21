using Microsoft.Extensions.Configuration;
using MochaMoth.Coincise.Core.Database;
using MochaMoth.Coincise.MongoDatabase.Model.Configuration;
using MongoDB.Driver;

namespace MochaMoth.Coincise.MongoDatabase.Database
{
	public class MongoDatabaseInitializer : IInitializeDatabase
	{
		public void Initialize(IConfigurationSection configuration)
		{
			MongoDatabaseConfiguration dbConfiguration = configuration.Get<MongoDatabaseConfiguration>()!;
			IMongoClient client = new MongoClient(dbConfiguration.ConnectionString);
			IMongoDatabase database = client.GetDatabase(dbConfiguration.DatabaseName);
		}
	}
}
