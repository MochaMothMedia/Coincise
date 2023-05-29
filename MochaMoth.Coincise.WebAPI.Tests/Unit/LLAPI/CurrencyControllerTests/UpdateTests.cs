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

namespace MochaMoth.Coincise.WebAPI.Tests.Unit.LLAPI.CurrencyControllerTests
{
	public class UpdateTests
	{
		private readonly CurrencyController _controller;
		private readonly IDatabase _database;
		private readonly ILog _logger;

		public UpdateTests()
		{
			_database = Substitute.For<IDatabase>();
			_logger = Substitute.For<ILog>();
			_controller = new CurrencyController(_database, _logger);
		}

		[Fact]
		public async Task Returns_OldCurrency_WithValidInput()
		{
			// Arrange
			Currency oldCurrency = new Currency()
			{
				Amount = 10,
				Type = CurrencyType.USD
			};
			Currency newCurrency = new Currency()
			{
				Amount = 20,
				Type = CurrencyType.USD
			};
			_ = _database.UpdateCurrency(Arg.Any<string>(), newCurrency).Returns(oldCurrency);

			// Act
			ActionResult<Currency?> result = await _controller.Update("someId", newCurrency);

			// Assert
			_ = await _database.Received(1).UpdateCurrency("someId", newCurrency);
			CreatedAtActionResult createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
			Assert.Equal("Get", createdAt.ActionName);
			Assert.Equal(201, createdAt.StatusCode);

			Currency? outputCurrency = Assert.IsType<Currency?>(createdAt.Value);
			Assert.NotNull(outputCurrency);
			Assert.Equal(oldCurrency, outputCurrency);
			Assert.NotNull(createdAt.RouteValues);

			string? outputId = Assert.IsType<string>(createdAt.RouteValues!["id"]);
			Assert.Equal("someId", outputId);
		}

		[Fact]
		public async Task Throws_NotFoundException_WhenUpdateReturnsNull()
		{
			// Arrange
			_ = _database.UpdateCurrency(Arg.Any<string>(), Arg.Any<Currency>()).ReturnsNull();

			// Act && Assert
			_ = await Assert.ThrowsAsync<NotFoundException>(() => _controller.Update("someId", new Currency()));
		}
	}
}
