using MochaMoth.Coincise.SystemModel;
using MochaMoth.Coincise.SystemModel.Constructs;

namespace MochaMoth.Coincise.Core.Database
{
	public interface IDatabase
	{
		Task<Currency?> GetCurrency(string id);
		Task<string> CreateCurrency(Currency currency);
		Task<Currency?> UpdateCurrency(string id, Currency currency);
		Task<Currency?> DeleteCurrency(string id);

		Task<Exchange?> GetExchange(string id);
		Task<string> CreateExchange(Exchange currency);
		Task<Exchange?> UpdateExchange(string id, Exchange currency);
		Task<Exchange?> DeleteExchange(string id);
	}
}
