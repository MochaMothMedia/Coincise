using MochaMoth.Coincise.MongoDatabase.Model;
using MongoDB.Driver;

namespace MochaMoth.Coincise.MongoDatabase.Operations
{
	public class CurrencyOperations
	{
		private readonly IMongoDatabase _database;
		private readonly IMongoCollection<Currency> _collection;

		public CurrencyOperations(IMongoDatabase database, IMongoCollection<Currency> collection)
		{
			_database = database;
			_collection = collection;
		}

		public async Task<string> Create(Currency currency)
		{
			//long foundCurrency = await _collection.FindAsync(cur => cur.Id == currency.Id);

			await _collection.InsertOneAsync(currency);

			return currency.Id!;
		}

		public async Task Update(Currency currency)
		{
			Currency result = await _collection.FindOneAndReplaceAsync<Currency>(currency.Id, currency);
		}
	}
}
