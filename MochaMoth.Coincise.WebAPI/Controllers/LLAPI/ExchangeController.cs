using Microsoft.AspNetCore.Mvc;
using MochaMoth.Coincise.Core.Database;
using MochaMoth.Coincise.Core.Logging;
using MochaMoth.Coincise.SystemModel.Constructs;
using MochaMoth.Coincise.WebAPI.Exceptions;

namespace MochaMoth.Coincise.WebAPI.Controllers.LLAPI
{
	[ApiController]
	[Route("llapi/[controller]")]
	public class ExchangeController : Controller
	{
		private readonly IDatabase _database;
		private readonly ILog _logger;

		public ExchangeController(IDatabase database, ILog logger)
		{
			_database = database;
			_logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult<Exchange?>> Get(string id)
		{
			Exchange? exchange = await _database.GetExchange(id);

			if (exchange == null)
				throw new NotFoundException();

			return exchange;
		}

		[HttpPost]
		public async Task<ActionResult<string>> Create(Exchange exchange)
		{
			string id = await _database.CreateExchange(exchange);

			if (id == null)
				throw new Exception("Unable to create exchange!");

			return CreatedAtAction(nameof(Get), new { id }, exchange);
		}

		[HttpPut]
		public async Task<ActionResult<Exchange?>> Update(string id, Exchange exchange)
		{
			Exchange? oldExchange = await _database.UpdateExchange(id, exchange);

			if (oldExchange == null)
				throw new NotFoundException();

			return CreatedAtAction(nameof(Get), new { id }, oldExchange);
		}

		[HttpDelete]
		public async Task<ActionResult<Exchange?>> Delete(string id)
		{
			Exchange? deletedExchange = await _database.DeleteExchange(id);

			if (deletedExchange == null)
				throw new NotFoundException();

			return deletedExchange;
		}
	}
}
