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
	public class DeleteTests
	{
		private readonly CurrencyController _controller;
		private readonly IDatabase _database;
		private readonly ILog _logger;

		public DeleteTests()
		{
			_database = Substitute.For<IDatabase>();
			_logger = Substitute.For<ILog>();
			_controller = new CurrencyController(_database, _logger);
		}

		[Fact]
		public async Task Returns_DeletedCurrency_WithValidId()
		{
			// Arrange
			Currency deletedCurrency = new Currency()
			{
				Amount = 20,
				Type = CurrencyType.USD
			};
			_ = _database.DeleteCurrency(Arg.Any<string>()).Returns(deletedCurrency);

			// Act
			ActionResult<Currency?> result = await _controller.Delete("someId");

			// Assert
			_ = await _database.Received(1).DeleteCurrency("someId");
			Currency? currency = Assert.IsType<Currency?>(result.Value);
			Assert.NotNull(currency);
			Assert.Equal(deletedCurrency, currency);
		}

		[Fact]
		public async Task Throws_NotFoundException_WhenDatabaseReturnsNull()
		{
			// Arrange
			_ = _database.DeleteCurrency(Arg.Any<string>()).ReturnsNull();

			// Act && Assert
			_ = await Assert.ThrowsAsync<NotFoundException>(() => _controller.Delete("someId"));
		}
	}
}
