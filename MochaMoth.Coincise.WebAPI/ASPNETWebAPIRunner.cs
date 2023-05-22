using MochaMoth.Coincise.Core.Database;
using MochaMoth.Coincise.Core.Logging;
using MochaMoth.Coincise.Core.WebAPI;
using MochaMoth.Coincise.WebAPI.Middleware;
using Swashbuckle.AspNetCore.Swagger;
using System.Text.Json.Serialization;

namespace MochaMoth.Coincise.WebAPI
{
	public class ASPNETWebAPIRunner : IAPIRunner
	{
		private readonly ILog _logger;
		private readonly IDatabase _database;

		public ASPNETWebAPIRunner(ILog logger, IDatabase database)
		{
			_logger = logger;
			_database = database;
		}

		public void Run()
		{
			_logger.LogInfo("Starting ASPNET Web Application.");

			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			AddServices(builder.Services);

			WebApplication app = builder.Build();

			ConfigurePipeline(app);
			ConfigureMiddleware(app);

			app.Run();

			_logger.LogInfo("ASPNET Web Application closed.");
		}

		private void AddServices(IServiceCollection services)
		{
			_ = services.AddSingleton((serviceProvider) => _logger);
			_ = services.AddScoped((serviceProvider) => _database);
			_ = services
				.AddControllers()
				.AddJsonOptions(x =>
				{
					x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
					x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
				});
			_ = services.AddCors();
			_ = services.AddEndpointsApiExplorer();
			_ = services.AddSwaggerGen();
		}

		private void ConfigureMiddleware(WebApplication app)
		{
			_ = app.UseMiddleware<ErrorHandlerMiddleware>();
		}

		private void ConfigurePipeline(WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				_logger.LogInfo("Development Environment - Adding Swagger.");
				_ = app.UseSwagger();
				_ = app.UseSwaggerUI();
			}

			_ = app.UseCors(cors => cors
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
			);

			_ = app.UseHttpsRedirection();
			_ = app.UseAuthorization();
			_ = app.MapControllers();
		}
	}
}