using System.Collections.Generic;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.Helpers;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.Controllers;

public class ProductControllerTests
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;

    private readonly ProductController _productController;

    public ProductControllerTests()
    {
        _logger = Mock.Of<ILogger<ProductController>>();
        _productService = Mock.Of<IProductService>();

        _productController = new ProductController(_logger, _productService);
    }

    [Fact]
    public void Get_WhenListOfProductsIsRequested_ReturnList()
    {
        // Arrange
        Mock.Get(_productService)
            .Setup(service => service.List(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(new List<Product> { new() });

        // Act
        var result = _productController.Get(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        
        Mock.Get(_productService)
            .Verify(service => service.List(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void Get_WhenListOfProductsWithCurrencyIsRequested_ReturnList()
    {
        // Arrange
        Mock.Get(_productService)
            .Setup(service => service.ListWithCurrency(It.IsAny<Currency>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(new List<ProductWithCurrency> { new() });

        // Act
        var result = _productController.Get(It.IsAny<Currency>(), It.IsAny<int>(), It.IsAny<int>());

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        
        Mock.Get(_productService)
            .Verify(service => service.ListWithCurrency(It.IsAny<Currency>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }
}