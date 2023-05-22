using MochaMoth.Coincise.Core.Logging;

namespace MochaMoth.Coincise.ConsoleLauncher.Logging
{
	internal class WarningLogger : ILogWarning
	{
		public void Log(string message)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[WARN ] - {message}");
			Console.ResetColor();
		}
	}
}
