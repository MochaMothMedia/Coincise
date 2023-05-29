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

namespace MochaMoth.Coincise.WebAPI.Tests.Unit.LLAPI.CurrencyControllerTests
{
	public class CreateTests
	{
		private readonly CurrencyController _controller;
		private readonly IDatabase _database;
		private readonly ILog _logger;

		public CreateTests()
		{
			_database = Substitute.For<IDatabase>();
			_logger = Substitute.For<ILog>();
			_controller = new CurrencyController(_database, _logger);
		}

		[Fact]
		public async Task Returns_CreatedCurrency_WithValidInput()
		{
			// Arrange
			Currency currency = new Currency()
			{
				Amount = 20,
				Type = CurrencyType.USD
			};
			_ = _database.CreateCurrency(currency).Returns("someId");

			// Act
			ActionResult<string> result = await _controller.Create(currency);

			// Assert
			_ = await _database.Received(1).CreateCurrency(currency);
			CreatedAtActionResult createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
			Assert.Equal("Get", createdAt.ActionName);
			Assert.Equal(201, createdAt.StatusCode);
			Assert.NotNull(createdAt.RouteValues);

			Currency? outputCurrency = Assert.IsType<Currency?>(createdAt.Value);
			Assert.NotNull(outputCurrency);
			Assert.Equal(currency, outputCurrency);

			string? outputId = Assert.IsType<string>(createdAt.RouteValues!["id"]);
			Assert.Equal("someId", outputId);
		}

		[Fact]
		public async Task Throws_Exception_WhenCreateReturnsNull()
		{
			// Arrange
			_ = _database.CreateCurrency(Arg.Any<Currency>()).ReturnsNull();

			// Act && Assert
			_ = await Assert.ThrowsAsync<Exception>(() => _controller.Create(new Currency()));
		}
	}
}
