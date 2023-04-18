namespace MochaMoth.Coincise.Core.Logging
{
	public class LogFacade : ILogFacade
	{
		private readonly ILogInfo _infoLogger;
		private readonly ILogWarning _warningLogger;
		private readonly ILogError _errorLogger;

		public LogFacade(
			ILogInfo infoLogger,
			ILogWarning warningLogger,
			ILogError errorLogger)
		{
			_infoLogger = infoLogger;
			_warningLogger = warningLogger;
			_errorLogger = errorLogger;
		}

		public void LogInfo(string message) => _infoLogger.Log(message);
		public void LogWarning(string message) => _warningLogger.Log(message);
		public void LogError(string message, Exception? exception = null) => _errorLogger.Log(message, exception);
	}
}
