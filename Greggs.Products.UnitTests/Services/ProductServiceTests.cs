using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Helpers;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Greggs.Products.Api.Services.Abstractions;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.Services;

public class ProductServiceTests
{
    private readonly IDataAccess<Product> _dataAccess;
    private readonly ICurrencyConverter _currencyConverter;

    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _dataAccess = Mock.Of<IDataAccess<Product>>();
        _currencyConverter = Mock.Of<ICurrencyConverter>();

        _productService = new ProductService(_dataAccess, _currencyConverter);
    }

    [Fact]
    public void List_WhenPageStartAndSizeAreProvided_ReturnListOfProducts()
    {
        // Arrange
        Mock.Get(_dataAccess)
            .Setup(d => d.List(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(new List<Product>
            {
                new(),
                new()
            });

        // Act
        var products = _productService.List(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        Assert.NotNull(products);
        Assert.Equal(2, products.Count());

        Mock.Get(_dataAccess)
            .Verify(d => d.List(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void ListWithCurrency_WhenNoProductsExist_ReturnEmptyEnumerable()
    {
        // Arrange
        Mock.Get(_dataAccess)
            .Setup(d => d.List(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Enumerable.Empty<Product>());

        // Act
        var products = _productService.ListWithCurrency(It.IsAny<Currency>(), It.IsAny<int>(), It.IsAny<int>());

        // Assert
        Assert.NotNull(products);
        Assert.Empty(products);

        Mock.Get(_dataAccess)
            .Verify(d => d.List(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        Mock.Get(_currencyConverter)
            .Verify(c => c.GetExchangeRateFromPounds(It.IsAny<Currency>()), Times.Never);
    }

    [Fact]
    public void ListWithCurrency_WhenGBPIsRequested_ReturnListOfProductsInGBP()
    {
        // Arrange
        const Currency requestedCurrency = Currency.GBP;
        var productsInDatabase = new List<Product>
        {
            new() { Name = "Product 1", PriceInPounds = 1 },
            new() { Name = "Product 2", PriceInPounds = 2 }
        };

        Mock.Get(_dataAccess)
            .Setup(d => d.List(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(productsInDatabase);
        Mock.Get(_currencyConverter)
            .Setup(c => c.GetExchangeRateFromPounds(requestedCurrency))
            .Returns(1);

        // Act
        var products = _productService.ListWithCurrency(requestedCurrency, It.IsAny<int>(), It.IsAny<int>());

        // Assert
        Assert.NotNull(products);

        var productsArray = products.ToArray();

        Assert.Equal(productsInDatabase.Count, productsArray.Length);
        for (var i = 0; i < productsInDatabase.Count; i++)
        {
            Assert.Equal(productsInDatabase[i].Name, productsArray[i].Name);
            Assert.Equal(productsInDatabase[i].PriceInPounds, productsArray[i].Price);
            Assert.Equal(requestedCurrency, productsArray[i].Currency);
        }

        Mock.Get(_dataAccess)
            .Verify(d => d.List(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        Mock.Get(_currencyConverter)
            .Verify(c => c.GetExchangeRateFromPounds(requestedCurrency), Times.Never);
    }
    
    [Fact]
    public void ListWithCurrency_WhenEURIsRequested_ReturnListOfProductsInEUR()
    {
        // Arrange
        const Currency requestedCurrency = Currency.EUR;
        const decimal exchangeRate = 1.5m;
        var productsInDatabase = new List<Product>
        {
            new() { Name = "Product 1", PriceInPounds = 1 },
            new() { Name = "Product 2", PriceInPounds = 2 }
        };

        Mock.Get(_dataAccess)
            .Setup(d => d.List(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(productsInDatabase);
        Mock.Get(_currencyConverter)
            .Setup(c => c.GetExchangeRateFromPounds(requestedCurrency))
            .Returns(exchangeRate);

        // Act
        var products = _productService.ListWithCurrency(requestedCurrency, It.IsAny<int>(), It.IsAny<int>());

        // Assert
        Assert.NotNull(products);

        var productsArray = products.ToArray();

        Assert.Equal(productsInDatabase.Count, productsArray.Length);
        for (var i = 0; i < productsInDatabase.Count; i++)
        {
            Assert.Equal(productsInDatabase[i].Name, productsArray[i].Name);
            Assert.NotEqual(productsInDatabase[i].PriceInPounds, productsArray[i].Price);
            Assert.Equal(productsArray[i].Price, productsInDatabase[i].PriceInPounds * exchangeRate);
            Assert.Equal(requestedCurrency, productsArray[i].Currency);
        }

        Mock.Get(_dataAccess)
            .Verify(d => d.List(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        Mock.Get(_currencyConverter)
            .Verify(c => c.GetExchangeRateFromPounds(requestedCurrency), Times.Exactly(productsInDatabase.Count));
    }
}