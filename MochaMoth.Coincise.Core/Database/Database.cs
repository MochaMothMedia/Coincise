using MochaMoth.Coincise.Core.Database.Operations;
using MochaMoth.Coincise.SystemModel;
using MochaMoth.Coincise.SystemModel.Constructs;

namespace MochaMoth.Coincise.Core.Database
{
	public class Database : IDatabase
	{
		private readonly ICurrencyOperations _currencyOperations;
		private readonly IExchangeOperations _exchangeOperations;

		public Database(ICurrencyOperations currencyOperations, IExchangeOperations exchangeOperations)
		{
			_currencyOperations = currencyOperations;
			_exchangeOperations = exchangeOperations;
		}

		public Task<Currency?> GetCurrency(string id) => _currencyOperations.Get(id);
		public Task<string> CreateCurrency(Currency currency) => _currencyOperations.Create(currency);
		public Task<Currency?> UpdateCurrency(string id, Currency currency) => _currencyOperations.Update(id, currency);
		public Task<Currency?> DeleteCurrency(string id) => _currencyOperations.Delete(id);

		public Task<Exchange?> GetExchange(string id) => _exchangeOperations.Get(id);
		public Task<string> CreateExchange(Exchange currency) => _exchangeOperations.Create(currency);
		public Task<Exchange?> UpdateExchange(string id, Exchange currency) => _exchangeOperations.Update(id, currency);
		public Task<Exchange?> DeleteExchange(string id) => _exchangeOperations.Delete(id);
	}
}
