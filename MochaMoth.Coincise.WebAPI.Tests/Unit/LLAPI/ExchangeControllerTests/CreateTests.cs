using Microsoft.AspNetCore.Mvc;
using MochaMoth.Coincise.Core.Database;
using MochaMoth.Coincise.Core.Logging;
using MochaMoth.Coincise.SystemModel.Constructs;
using MochaMoth.Coincise.WebAPI.Controllers.LLAPI;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MochaMoth.Coincise.WebAPI.Tests.Unit.LLAPI.ExchangeControllerTests
{
	public class CreateTests
	{
		private readonly ExchangeController _controller;
		private readonly IDatabase _database;
		private readonly ILog _logger;

		public CreateTests()
		{
			_database = Substitute.For<IDatabase>();
			_logger = Substitute.For<ILog>();
			_controller = new ExchangeController(_database, _logger);
		}

		[Fact]
		public async Task Returns_CreatedID_WithValidInput()
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
			_ = _database.CreateExchange(exchange).Returns("someId");

			// Act
			ActionResult<string> result = await _controller.Create(exchange);

			// Assert
			_ = await _database.Received(1).CreateExchange(exchange);
			CreatedAtActionResult createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
			Assert.Equal("Get", createdAt.ActionName);
			Assert.Equal(201, createdAt.StatusCode);
			Assert.NotNull(createdAt.RouteValues);

			Exchange? outputExchange = Assert.IsType<Exchange?>(createdAt.Value);
			Assert.NotNull(outputExchange);
			Assert.Equal(exchange, outputExchange);

			string? outputId = Assert.IsType<string>(createdAt.RouteValues!["id"]);
			Assert.Equal("someId", outputId);
		}

		[Fact]
		public async Task Throws_Exception_WhenCreateReturnsNull()
		{
			// Arrange
			_ = _database.CreateExchange(Arg.Any<Exchange>()).ReturnsNull();

			// Act && Assert
			_ = await Assert.ThrowsAsync<Exception>(() => _controller.Create(new Exchange()));
		}
	}
}
