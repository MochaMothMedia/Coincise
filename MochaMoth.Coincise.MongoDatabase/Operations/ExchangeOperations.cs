using MochaMoth.Coincise.Core.Database.Operations;
using MochaMoth.Coincise.Core.Logging;
using MochaMoth.Coincise.MongoDatabase.Model;
using MochaMoth.Coincise.SystemModel.Constructs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MochaMoth.Coincise.MongoDatabase.Operations
{
	public class ExchangeOperations : IExchangeOperations
	{
		private readonly IMongoCollection<MongoDB_Exchange> _collection;
		private readonly ICurrencyOperations _currencyOperations;
		private readonly ILog _logger;

		public ExchangeOperations(
			IMongoCollection<MongoDB_Exchange> collection,
			ICurrencyOperations currencyOperations,
			ILog logger)
		{
			_collection = collection;
			_currencyOperations = currencyOperations;
			_logger = logger;
		}

		#region IExchangeOperations Implementations
		public async Task<Exchange?> Get(string id)
		{
			_logger.LogInfo($"Getting Exchange with ID '{id}'.");
			MongoDB_Exchange? foundExchange = await GetFromMongoDB(id);

			return await GetExchangeFromDatabase(foundExchange);
		}

		public async Task<string> Create(Exchange exchange)
		{
			if (exchange.SourceCurrency == null || exchange.TransactionCurrency == null)
			{
				throw new MongoClientException($"Exchange arrived with null fields.");
			}

			_logger.LogInfo($"Creating Exchange from {exchange.TransactionCurrency.Amount} to {exchange.SourceCurrency.Amount}");

			string sourceId = await _currencyOperations.Create(exchange.SourceCurrency);
			string transactionId = await _currencyOperations.Create(exchange.TransactionCurrency);

			MongoDB_Exchange mongoExchange = exchange;
			mongoExchange.SourceCurrencyId = sourceId;
			mongoExchange.TransactionCurrencyId = transactionId;

			await _collection.InsertOneAsync(mongoExchange);
			_logger.LogInfo($"Exchange created with ID '{mongoExchange.Id}'.");
			return mongoExchange.Id!;
		}

		public async Task<Exchange?> Update(string id, Exchange exchange)
		{
			MongoDB_Exchange? foundExchange = await GetFromMongoDB(id);
			Exchange? priorExchange = await GetExchangeFromDatabase(foundExchange);

			if (foundExchange == null || priorExchange == null)
			{
				_logger.LogWarning($"Exchange with ID {id} does not exist.");
				return null;
			}

			if (exchange.SourceCurrency != null)
			{
				_ = _currencyOperations.Update(foundExchange.SourceCurrencyId, exchange.SourceCurrency);
			}

			if (exchange.TransactionCurrency != null)
			{
				_ = _currencyOperations.Update(foundExchange.TransactionCurrencyId, exchange.TransactionCurrency);
			}

			return priorExchange;
		}

		public async Task<Exchange?> Delete(string id)
		{
			_logger.LogInfo($"Deleting Exchange with ID '{id}'.");
			MongoDB_Exchange? foundExchange = await GetFromMongoDB(id);
			Exchange? priorExchange = await GetExchangeFromDatabase(foundExchange);

			if (foundExchange == null || priorExchange == null)
			{
				_logger.LogWarning($"Exchange with ID {id} does not exist.");
				return null;
			}

			if (foundExchange.SourceCurrencyId != null)
			{
				_ = await _currencyOperations.Delete(foundExchange.SourceCurrencyId);
			}

			if (foundExchange.TransactionCurrencyId != null)
			{
				_ = await _currencyOperations.Delete(foundExchange.TransactionCurrencyId);
			}

			FilterDefinition<MongoDB_Exchange> idFilter = Builders<MongoDB_Exchange>.Filter.Eq("_id", ObjectId.Parse(id));
			_ = await _collection.FindOneAndDeleteAsync(idFilter);

			_logger.LogInfo($"Deleted Exchange '{id}'.");
			return priorExchange;
		}
		#endregion

		#region Database Utilities
		private async Task<MongoDB_Exchange?> GetFromMongoDB(string id)
		{
			FilterDefinition<MongoDB_Exchange> idFilter = Builders<MongoDB_Exchange>.Filter.Eq("_id", ObjectId.Parse(id));
			IAsyncCursor<MongoDB_Exchange>? currencyCursor = await _collection.FindAsync(idFilter);
			MongoDB_Exchange? foundExchange = await currencyCursor.FirstOrDefaultAsync();

			if (foundExchange == null)
			{
				_logger.LogWarning($"Exchange with ID '{id}' was not found!");
				return null;
			}

			_logger.LogInfo($"Exchange found with ID '{id}'.");
			return foundExchange;
		}

		private async Task<Exchange?> GetExchangeFromDatabase(MongoDB_Exchange? mongoDBExchange)
		{
			if (mongoDBExchange == null)
			{
				return null;
			}

			Exchange exchange = mongoDBExchange;

			exchange.SourceCurrency = await _currencyOperations.Get(mongoDBExchange.SourceCurrencyId);
			exchange.TransactionCurrency = await _currencyOperations.Get(mongoDBExchange.TransactionCurrencyId);

			return exchange;
		}
		#endregion
	}
}
