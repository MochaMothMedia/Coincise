using MochaMoth.Coincise.SystemModel;

namespace MochaMoth.Coincise.Core.Database
{
	public interface IDatabase
	{
		Task<Currency?> GetCurrency(string id);
		Task<string> CreateCurrency(Currency currency);
		Task<Currency?> UpdateCurrency(string id, Currency currency);
		Task<Currency?> DeleteCurrency(string id);
	}
}
