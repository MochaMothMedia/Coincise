using MochaMoth.Coincise.MongoDatabase.Model.Configuration.Collections;

namespace MochaMoth.Coincise.MongoDatabase.Model.Configuration
{
	public class MongoDatabaseConfiguration
	{
		public string ConnectionString { get; set; } = null!;
		public string DatabaseName { get; set; } = null!;
		public MongoDatabaseCollections Collections { get; set; } = null!;
	}
}
