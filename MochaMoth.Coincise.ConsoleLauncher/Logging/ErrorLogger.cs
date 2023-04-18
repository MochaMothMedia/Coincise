using MochaMoth.Coincise.Core.Logging;

namespace MochaMoth.Coincise.ConsoleLauncher.Logging
{
	internal class ErrorLogger : ILogError
	{
		public void Log(string message, Exception? exception = null)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			if (exception != null)
				Console.WriteLine(exception.StackTrace);
			Console.ResetColor();
		}
	}
}
