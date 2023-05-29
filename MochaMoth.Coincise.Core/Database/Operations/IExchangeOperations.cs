using MochaMoth.Coincise.SystemModel.Constructs;

namespace MochaMoth.Coincise.Core.Database.Operations
{
	public interface IExchangeOperations
	{
		Task<Exchange?> Get(string id);
		Task<string> Create(Exchange currency);
		Task<Exchange?> Update(string id, Exchange currency);
		Task<Exchange?> Delete(string id);
	}
}
