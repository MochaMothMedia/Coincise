using MochaMoth.Coincise.SystemModel.Constructs;

namespace MochaMoth.Coincise.Core.Database.Operations
{
	public interface ICurrencyOperations
	{
		Task<Currency?> Get(string id);
		Task<string> Create(Currency currency);
		Task<Currency?> Update(string id, Currency currency);
		Task<Currency?> Delete(string id);
	}
}
