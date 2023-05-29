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
	public class DeleteTests
	{
		private readonly ExchangeController _controller;
		private readonly IDatabase _database;
		private readonly ILog _logger;

		public DeleteTests()
		{
			_database = Substitute.For<IDatabase>();
			_logger = Substitute.For<ILog>();
			_controller = new ExchangeController(_database, _logger);
		}

		[Fact]
		public async Task Returns_DeletedExchange_WithValidID()
		{
			// Arrange
			Exchange deletedExchange = new Exchange()
			{
				SourceCurrency = new Currency()
				{
					Amount = 10,
					Type = CurrencyType.USD
				},
				TransactionCurrency = new Currency()
				{
					Amount = 10,
					Type = CurrencyType.USD
				}
			};
			_ = _database.DeleteExchange("someId").Returns(deletedExchange);

			// Act
			ActionResult<Exchange?> result = await _controller.Delete("someId");

			// Assert
			_ = await _database.Received(1).DeleteExchange("someId");
			Exchange? exchange = Assert.IsType<Exchange?>(result.Value);
			Assert.NotNull(exchange);
			Assert.Equal(deletedExchange, exchange);
		}

		[Fact]
		public async Task Throws_NotFoundException_WhenDatabaseReturnsNull()
		{
			// Arrange
			_ = _database.DeleteExchange(Arg.Any<string>()).ReturnsNull();

			// Act && Assert
			_ = await Assert.ThrowsAsync<NotFoundException>(() => _controller.Delete("someId"));
		}
	}
}
