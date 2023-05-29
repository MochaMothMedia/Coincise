using MochaMoth.Coincise.Core.Logging;

namespace MochaMoth.Coincise.ConsoleLauncher.Logging
{
	internal class InfoLogger : ILogInfo
	{
		public void Log(string message)
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine($"({DateTime.UtcNow.TimeOfDay}) [INFO ] - {message}");
			Console.ResetColor();
		}
	}
}
