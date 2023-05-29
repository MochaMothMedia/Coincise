using MochaMoth.Coincise.SystemModel.Constructs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MochaMoth.Coincise.MongoDatabase.Model
{
	public class MongoDB_Exchange
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string SourceCurrencyId { get; set; } = null!;

		[BsonRepresentation(BsonType.ObjectId)]
		public string TransactionCurrencyId { get; set; } = null!;

		public static implicit operator Exchange(MongoDB_Exchange mongoExchange)
		{
			return new Exchange() { };
		}

		public static implicit operator MongoDB_Exchange(Exchange exchange)
		{
			return new MongoDB_Exchange() { };
		}
	}
}
