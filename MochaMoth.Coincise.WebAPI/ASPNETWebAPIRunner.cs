using MochaMoth.Coincise.Core.Logging;
using MochaMoth.Coincise.Core.WebAPI;

namespace MochaMoth.Coincise.WebAPI
{
	public class ASPNETWebAPIRunner : IAPIRunner
	{
		private readonly ILogFacade _logger;

		public ASPNETWebAPIRunner(ILogFacade logger)
		{
			_logger = logger;
		}

		public void Run()
		{
			_logger.LogInfo("Starting ASPNET Web Application.");

			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			AddServices(builder);

			WebApplication app = builder.Build();

			ConfigurePipeline(app);

			app.Run();

			_logger.LogInfo("ASPNET Web Application closed.");
		}

		private void AddServices(WebApplicationBuilder builder)
		{
			_ = builder.Services.AddControllers();
			_ = builder.Services.AddEndpointsApiExplorer();
			_ = builder.Services.AddSwaggerGen();
		}

		private void ConfigurePipeline(WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				_logger.LogInfo("Development Environment - Adding Swagger.");
				_ = app.UseSwagger();
				_ = app.UseSwaggerUI();
			}

			_ = app.UseHttpsRedirection();
			_ = app.UseAuthorization();
			_ = app.MapControllers();
		}
	}
}