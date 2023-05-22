using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MochaMoth.Coincise.Core.Database.Operations;
using MochaMoth.Coincise.MongoDatabase.Model;
using MochaMoth.Coincise.MongoDatabase.Model.Configuration;
using MochaMoth.Coincise.MongoDatabase.Operations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace MochaMoth.Coincise.MongoDatabase.Database
{
	public class MongoDatabaseInitializer
	{
		public static void Initialize(IConfigurationSection configuration, IServiceCollection services)
		{
			var pack = new ConventionPack
			{
				new EnumRepresentationConvention(BsonType.String)
			};

			ConventionRegistry.Register("EnumStringConvention", pack, t => true);

			MongoDatabaseConfiguration dbConfiguration = configuration.Get<MongoDatabaseConfiguration>()!;

			IMongoClient client = new MongoClient(dbConfiguration.ConnectionString);
			IMongoDatabase database = client.GetDatabase(dbConfiguration.DatabaseName);

			IMongoCollection<MongoDB_Currency> currencyCollection = database.GetCollection<MongoDB_Currency>(dbConfiguration.Collections.Currency);

			_ = services.AddScoped((services) => currencyCollection);

			AddOperations(services);
		}

		public static void AddOperations(IServiceCollection services)
		{
			_ = services.AddScoped<ICurrencyOperations, CurrencyOperations>();
		}
	}
}
