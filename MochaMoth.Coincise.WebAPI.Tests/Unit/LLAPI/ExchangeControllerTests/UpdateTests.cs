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
	public class UpdateTests
	{
		private readonly ExchangeController _controller;
		private readonly IDatabase _database;
		private readonly ILog _logger;

		public UpdateTests()
		{
			_database = Substitute.For<IDatabase>();
			_logger = Substitute.For<ILog>();
			_controller = new ExchangeController(_database, _logger);
		}

		[Fact]
		public async Task Returns_OldCurrency_WithValidInput()
		{
			// Arrange
			Exchange oldExchange = new Exchange()
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
			Exchange newExchange = new Exchange()
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
			_ = _database.UpdateExchange(Arg.Any<string>(), newExchange).Returns(oldExchange);

			// Act
			ActionResult<Exchange?> result = await _controller.Update("someId", newExchange);

			// Assert
			_ = await _database.Received(1).UpdateExchange("someId", newExchange);
			CreatedAtActionResult createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
			Assert.Equal("Get", createdAt.ActionName);
			Assert.Equal(201, createdAt.StatusCode);
			Assert.NotNull(createdAt.RouteValues);

			Exchange? outputExchange = Assert.IsType<Exchange?>(createdAt.Value);
			Assert.NotNull(outputExchange);
			Assert.Equal(oldExchange, outputExchange);

			string? outputId = Assert.IsType<string>(createdAt.RouteValues!["id"]);
			Assert.Equal("someId", outputId);
		}

		[Fact]
		public async Task Throws_NotFoundException_WhenDatabaseReturnsNull()
		{
			// Arrange
			_ = _database.UpdateExchange(Arg.Any<string>(), Arg.Any<Exchange>()).ReturnsNull();

			// Act && Assert
			_ = await Assert.ThrowsAsync<NotFoundException>(() => _controller.Update("someId", new Exchange()));
		}
	}
}
