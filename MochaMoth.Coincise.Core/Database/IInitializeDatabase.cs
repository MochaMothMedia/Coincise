using Microsoft.Extensions.Configuration;

namespace MochaMoth.Coincise.Core.Database
{
	public interface IInitializeDatabase
	{
		void Initialize(IConfigurationSection configuration);
	}
}
