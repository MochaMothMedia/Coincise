namespace MochaMoth.Coincise.Core.Logging
{
	public interface ILogError
	{
		void Log(string message, Exception? exception = null);
	}
}
