using Microsoft.AspNetCore.Mvc;
using MochaMoth.Coincise.Core.Database;
using MochaMoth.Coincise.Core.Logging;
using MochaMoth.Coincise.SystemModel;
using MochaMoth.Coincise.WebAPI.Exceptions;

namespace MochaMoth.Coincise.WebAPI.Controllers.LLAPI
{
	[ApiController]
	[Route("llapi/[controller]")]
	public class CurrencyController : Controller
	{
		private readonly IDatabase _database;
		private readonly ILog _logger;

		public CurrencyController(IDatabase database, ILog logger)
		{
			_database = database;
			_logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult<Currency?>> Get(string id)
		{
			Currency? currency = await _database.GetCurrency(id);

			if (currency == null)
				throw new NotFoundException();

			return currency;
		}

		[HttpPost]
		public async Task<ActionResult<string>> Create(Currency currency)
		{
			string id = await _database.CreateCurrency(currency);

			if (id == null)
				throw new Exception("Unable to create currency!");

			return CreatedAtAction(nameof(Get), new { id }, currency);
		}

		[HttpPut]
		public async Task<ActionResult<Currency?>> Update(string id, Currency currency)
		{
			Currency? oldCurrency = await _database.UpdateCurrency(id, currency);

			if (oldCurrency == null)
				throw new NotFoundException();

			return CreatedAtAction(nameof(Get), new { id }, currency);
		}

		[HttpDelete]
		public async Task<ActionResult<Currency?>> Delete(string id)
		{
			Currency? deletedCurrency = await _database.DeleteCurrency(id);

			if (deletedCurrency == null)
				throw new NotFoundException();

			return deletedCurrency;
		}
	}
}
