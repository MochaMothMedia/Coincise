namespace MochaMoth.Coincise.Core.WebAPI
{
	public class APIFacade : IAPIFacade
	{
		private readonly IAPIRunner _runner;

		public APIFacade(IAPIRunner runner)
		{
			_runner = runner;
		}

		public void RunAPI() => _runner.Run();
	}
}
