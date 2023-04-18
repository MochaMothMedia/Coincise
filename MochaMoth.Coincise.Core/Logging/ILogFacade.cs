namespace MochaMoth.Coincise.Core.Logging
{
	public interface ILogFacade
	{
		void LogInfo(string message);
		void LogWarning(string message);
		void LogError(string message, Exception? exception = null);
	}
}
