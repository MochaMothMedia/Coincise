using MochaMoth.Coincise.SystemModel.Constructs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MochaMoth.Coincise.MongoDatabase.Model
{
	public class MongoDB_Currency
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
		
		public CurrencyType Type { get; set; }
		
		public double Amount { get; set; }

		public static implicit operator Currency(MongoDB_Currency mongoCurrency)
		{
			return new Currency()
			{
				Type = mongoCurrency.Type,
				Amount = mongoCurrency.Amount
			};
		}

		public static implicit operator MongoDB_Currency(Currency currency)
		{
			return new MongoDB_Currency()
			{
				Type = currency.Type,
				Amount = currency.Amount
			};
		}
	}
}
