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
	public class GetTests
	{
		private readonly CurrencyController _controller;
		private readonly IDatabase _database;
		private readonly ILog _logger;

		public GetTests()
		{
			_database = Substitute.For<IDatabase>();
			_logger = Substitute.For<ILog>();
			_controller = new CurrencyController(_database, _logger);
		}

		[Fact]
		public async Task Returns_ValidResult()
		{
			// Arrange
			Currency currency = new Currency()
			{
				Amount = 20,
				Type = CurrencyType.USD
			};
			_ = _database.GetCurrency(Arg.Any<string>()).Returns(currency);

			// Act
			ActionResult<Currency?> result = await _controller.Get("someId");

			// Assert
			_ = await _database.Received(1).GetCurrency(Arg.Any<string>());
			Currency? outputCurrency = Assert.IsType<Currency?>(result.Value);
			Assert.Equal(currency, outputCurrency);
		}

		[Fact]
		public async Task Throws_NotFoundException_WhenIDNotFound()
		{
			// Arrange
			_ = _database.GetCurrency(Arg.Any<string>()).ReturnsNull();

			// Act && Assert
			_ = await Assert.ThrowsAsync<NotFoundException>(() => _controller.Get("someId"));
		}
	}
}
