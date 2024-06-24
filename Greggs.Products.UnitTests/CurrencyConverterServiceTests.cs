using Xunit;
using Moq;
using FluentAssertions;
using Greggs.Products.Api.Models;
using System;
using Greggs.Products.Api.Services.Interfaces;
using Greggs.Products.Api.Services.Concrete;

namespace Greggs.Products.UnitTests
{
    public class CurrencyConverterServiceTests
    {
        [Fact]
        public void Convert_WhenCalledWithEURCurrency_ReturnsCorrectValue()
        {
            // Arrange
            decimal gbp = 1m;
            decimal eur = 1.11m;

            var currencyConverter = new CurrencyConverterService();
            var currency = Currency.EUR;

            // Act
            var result = currencyConverter.Convert(gbp, currency);

            // Assert
            result.Should().Be(eur);
        }

    }
}
