using MochaMoth.Coincise.Core.Database.Operations;
using MochaMoth.Coincise.SystemModel;

namespace MochaMoth.Coincise.Core.Database
{
	public class Database : IDatabase
	{
		private readonly ICurrencyOperations _currencyOperations;

		public Database(ICurrencyOperations currencyOperations)
		{
			_currencyOperations = currencyOperations;
		}

		public Task<Currency?> GetCurrency(string id) => _currencyOperations.Get(id);
		public Task<string> CreateCurrency(Currency currency) => _currencyOperations.Create(currency);
		public Task<Currency?> UpdateCurrency(string id, Currency currency) => _currencyOperations.Update(id, currency);
		public Task<Currency?> DeleteCurrency(string id) => _currencyOperations.Delete(id);
	}
}
