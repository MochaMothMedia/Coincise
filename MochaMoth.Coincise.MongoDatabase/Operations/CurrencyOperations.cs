using MochaMoth.Coincise.Core.Database.Operations;
using MochaMoth.Coincise.Core.Logging;
using MochaMoth.Coincise.MongoDatabase.Model;
using MochaMoth.Coincise.SystemModel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MochaMoth.Coincise.MongoDatabase.Operations
{
	public class CurrencyOperations : ICurrencyOperations
	{
		private readonly IMongoCollection<MongoDB_Currency> _collection;
		private readonly ILog _logger;

		public CurrencyOperations(IMongoCollection<MongoDB_Currency> collection, ILog logger)
		{
			_collection = collection;
			_logger = logger;
		}

		public async Task<Currency?> Get(string id)
		{
			_logger.LogInfo($"Getting Currency with ID '{id}'.");
			FilterDefinition<MongoDB_Currency> idFilter = Builders<MongoDB_Currency>.Filter.Eq("_id", ObjectId.Parse(id));
			IAsyncCursor<MongoDB_Currency>? currencyCursor = await _collection.FindAsync(idFilter);
			MongoDB_Currency? foundCurrency = await currencyCursor.FirstOrDefaultAsync();

			if (foundCurrency == null)
			{
				_logger.LogWarning($"Currency with ID '{id}' was not found!");
				return null;
			}

			_logger.LogInfo($"Currency found with ID '{id}'.");
			return foundCurrency;
		}

		public async Task<string> Create(Currency currency)
		{
			MongoDB_Currency mongoCurrency = currency;

			_logger.LogInfo($"Creating Currency with Type '{mongoCurrency.Type}' and Amount '{mongoCurrency.Amount}'.");
			await _collection.InsertOneAsync(mongoCurrency);
			_logger.LogInfo($"Currency created with ID '{mongoCurrency.Id}'.");
			return mongoCurrency.Id!;
		}

		public async Task<Currency?> Update(string id, Currency currency)
		{
			MongoDB_Currency mongoCurrency = currency;
			mongoCurrency.Id = id;

			_logger.LogInfo($"Updating Currency with ID '{id}'.");
			FilterDefinition<MongoDB_Currency> idFilter = Builders<MongoDB_Currency>.Filter.Eq("_id", ObjectId.Parse(id));
			MongoDB_Currency? priorCurrency = await _collection.FindOneAndReplaceAsync<MongoDB_Currency>(idFilter, mongoCurrency);

			if (priorCurrency == null)
			{
				_logger.LogWarning($"Unable to find Currency with ID '{id}'.");
				return null;
			}

			_logger.LogInfo($"Updated Currency with ID '{priorCurrency.Id}'.");
			return priorCurrency;
		}

		public async Task<Currency?> Delete(string id)
		{
			_logger.LogInfo($"Deleting Currency with ID '{id}'.");
			FilterDefinition<MongoDB_Currency> idFilter = Builders<MongoDB_Currency>.Filter.Eq("_id", ObjectId.Parse(id));
			MongoDB_Currency? deletedCurrency = await _collection.FindOneAndDeleteAsync(idFilter);

			if (deletedCurrency == null)
			{
				_logger.LogWarning($"Unable to find Currency with ID '{id}'.");
				return null;
			}

			_logger.LogInfo($"Deleted '{id}'.");
			return deletedCurrency;
		}
	}
}
