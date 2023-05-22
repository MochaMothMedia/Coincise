using MochaMoth.Coincise.APIModel;
using MochaMoth.Coincise.Core.Logging;
using MochaMoth.Coincise.WebAPI.Exceptions;
using System.Net;

namespace MochaMoth.Coincise.WebAPI.Middleware
{
	public class ErrorHandlerMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILog _logger;

		public ErrorHandlerMiddleware(RequestDelegate next, ILog logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (NotFoundException notFoundException)
			{
				_logger.LogError(notFoundException.Message, notFoundException);

				await SendResponse(
					context,
					(int)HttpStatusCode.NotFound,
					notFoundException.Message
				);
			}
			catch (FormatException formatException)
			{
				_logger.LogError(formatException.Message, formatException);

				await SendResponse(
					context,
					(int)HttpStatusCode.BadRequest,
					formatException.Message
				);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.Message, exception);

				await SendResponse(
					context,
					(int)HttpStatusCode.InternalServerError,
					"An unexpected error has occured"
				);
			}
		}

		private static async Task SendResponse(HttpContext context, int statusCode, string message)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = statusCode;

			await context.Response.WriteAsync(
				new ErrorDetails()
				{
					StatusCode = context.Response.StatusCode,
					Message = message,
				}.ToString()!
			);
		}
	}
}