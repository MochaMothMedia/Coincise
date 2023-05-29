using Microsoft.AspNetCore.Mvc;
using MochaMoth.Coincise.Core.Database;
using MochaMoth.Coincise.Core.Logging;
using MochaMoth.Coincise.SystemModel.Constructs;
using MochaMoth.Coincise.WebAPI.Controllers.LLAPI;
using MochaMoth.Coincise.WebAPI.Exceptions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Threading.Tasks;
using Xunit;

namespace MochaMoth.Coincise.WebAPI.Tests.Unit.LLAPI.ExchangeControllerTests
{
	public class GetTests
	{
		private readonly ExchangeController _controller;
		private readonly IDatabase _database;
		private readonly ILog _logger;

		public GetTests()
		{
			_database = Substitute.For<IDatabase>();
			_logger = Substitute.For<ILog>();
			_controller = new ExchangeController(_database, _logger);
		}

		[Fact]
		public async Task Returns_Exchange_WithValidID()
		{
			// Arrange
			Exchange exchange = new Exchange()
			{
				SourceCurrency = new Currency()
				{
					Amount = 20,
					Type = CurrencyType.USD
				},
				TransactionCurrency = new Currency()
				{
					Amount = 20,
					Type = CurrencyType.USD
				}
			};
			_ = _database.GetExchange("someId").Returns(exchange);

			// Act
			ActionResult<Exchange?> result = await _controller.Get("someId");

			// Assert
			_ = await _database.Received(1).GetExchange("someId");
			Exchange? returnedExchange = Assert.IsType<Exchange?>(result.Value);
			Assert.NotNull(returnedExchange);
			Assert.Equal(exchange, returnedExchange);
		}

		[Fact]
		public async Task Throws_NotFoundException_WhenDatabaseReturnsNull()
		{
			// Arrange
			_ = _database.GetExchange("someId").ReturnsNull();

			// Act && Assert
			_ = await Assert.ThrowsAsync<NotFoundException>(() => _controller.Get("someId"));
		}
	}
}
