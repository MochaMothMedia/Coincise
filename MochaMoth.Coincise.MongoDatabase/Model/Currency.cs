using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MochaMoth.Coincise.MongoDatabase.Model
{
	public class Currency
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
		
		public string Type { get; set; } = null!;
		
		public string Amount { get; set; } = null!;
	}
}
