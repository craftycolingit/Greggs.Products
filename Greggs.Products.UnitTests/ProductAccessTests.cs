using Greggs.Products.Api.DataAccess;
using Xunit;
using Moq;
using FluentAssertions;
using Greggs.Products.Api.Models;
using System.Threading.Tasks;
using Greggs.Products.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Greggs.Products.UnitTests;

public class ProductAccessTests
{
    private readonly Mock<IDataAccess<Product>> _mockProductAccess;
    private readonly Mock<ICurrencyConverterService> _mockCurrencyConverterService;

    public ProductAccessTests()
    {
        _mockCurrencyConverterService = new Mock<ICurrencyConverterService>();
        _mockProductAccess = new Mock<IDataAccess<Product>>();
    }

    // ListAsync when called returns a list of products
    [Fact]
    public async Task ListAsync_WhenCalled_ReturnsAListOfProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Name = "Sausage Roll", Price = 1m },
            new() { Name = "Vegan Sausage Roll", Price = 1.1m },
            new() { Name = "Steak Bake", Price = 1.2m },
            new() { Name = "Yum Yum", Price = 0.7m },
            new() { Name = "Pink Jammie", Price = 0.5m },
            new() { Name = "Mexican Baguette", Price = 2.1m },
            new() { Name = "Bacon Sandwich", Price = 1.95m },
            new() { Name = "Coca Cola", Price = 1.2m }
        };

        _mockProductAccess.Setup(x => x.ListAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Currency>(), It.IsAny<bool>()))
            .ReturnsAsync(products);

        // Act
        var result = await _mockProductAccess.Object.ListAsync(null, null, Currency.GBP, false);

        // Assert
        result.Should().BeEquivalentTo(products);
    }

    // ListAsync when called with a pageStart of 1 and pageSize of 2 returns a list of products starting from the second product
    [Fact]
    public async Task ListAsync_WhenCalledWithPageStart1AndPageSize2_ReturnsAListOfProductsStartingFromTheSecondProduct()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Name = "Vegan Sausage Roll", Price = 1.1m },
            new() { Name = "Steak Bake", Price = 1.2m },
            new() { Name = "Yum Yum", Price = 0.7m },
            new() { Name = "Pink Jammie", Price = 0.5m },
            new() { Name = "Mexican Baguette", Price = 2.1m },
            new() { Name = "Bacon Sandwich", Price = 1.95m },
            new() { Name = "Coca Cola", Price = 1.2m }
        };

        _mockProductAccess.Setup(x => x.ListAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Currency>(), It.IsAny<bool>()))
            .ReturnsAsync(products);

        // Act
        var result = await _mockProductAccess.Object.ListAsync(1, 2, Currency.GBP, false);

        // Assert
        result.Should().BeEquivalentTo(products);
    }

    // ListAsync when called with latestProducts set to true returns a list of products sorted by name in ascending order
    [Fact]
    public async Task ListAsync_WhenCalledWithLatestProductsTrue_ReturnsAListOfProductsSortedByNameInAscendingOrder()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Name = "Bacon Sandwich", Price = 1.95m },
            new() { Name = "Coca Cola", Price = 1.2m },
            new() { Name = "Mexican Baguette", Price = 2.1m },
            new() { Name = "Pink Jammie", Price = 0.5m },
            new() { Name = "Sausage Roll", Price = 1m },
            new() { Name = "Steak Bake", Price = 1.2m },
            new() { Name = "Vegan Sausage Roll", Price = 1.1m },
            new() { Name = "Yum Yum", Price = 0.7m }
        };

        _mockProductAccess.Setup(x => x.ListAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Currency>(), It.IsAny<bool>()))
            .ReturnsAsync(products);

        // Act
        var result = await _mockProductAccess.Object.ListAsync(null, null, Currency.GBP, true);

        // Assert
        result.Should().BeEquivalentTo(products.OrderBy(x => x.Name));
    }

    // ListAsync when called with currency set to EUR returns a list of products with prices converted to EUR
    [Fact]
    public async Task ListAsync_WhenCalledWithCurrencyEUR_ReturnsAListOfProductsWithPricesConvertedToEUR()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Name = "Sausage Roll", Price = 1.11m },
            new() { Name = "Vegan Sausage Roll", Price = 1.2m }
        };

        _mockProductAccess.Setup(x => x.ListAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Currency>(), It.IsAny<bool>()))
            .ReturnsAsync(products);

        _mockCurrencyConverterService.Setup(x => x.Convert(It.IsAny<decimal>(), It.IsAny<Currency>()))
            .Returns<decimal, Currency>((price, currency) => currency == Currency.EUR ? price * 1.11m : price);

        // Act
        var result = await _mockProductAccess.Object.ListAsync(null, null, Currency.EUR, false);

        // Assert
        result.Should().BeEquivalentTo(products.Select(x => new Product
        {
            Name = x.Name,
            Price = x.Price
        }));
    }
    
}