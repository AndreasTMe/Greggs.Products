using System.Threading.Tasks;
using Greggs.Products.Api.Helpers;
using Greggs.Products.Api.Models;
using Newtonsoft.Json;
using Xunit;

namespace Greggs.Products.IntegrationTests.Controllers;

public class ProductControllerTests : IntegrationTests
{
    [Fact]
    public async Task ProductEndpoint_When3ProductsAreRequested_ReturnListWith3Products()
    {
        // Arrange
        const string pageStart = "0";
        const string pageSize = "3";
    
        // Act
        var response = await Client.GetAsync($"product?pageStart={pageStart}&pageSize={pageSize}");
    
        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
    
        var contentAsString = await response.Content.ReadAsStringAsync();
        var products = JsonConvert.DeserializeObject<Product[]>(contentAsString);
        
        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.Equal(int.Parse(pageSize), products.Length);
    }
    
    [Fact]
    public async Task ProductCurrencyEndpoint_WhenProductsAreRequestedInEUR_ReturnItemsWithCorrectCurrency()
    {
        // Arrange
        const string pageStart = "0";
        const string pageSize = "3";
        const Currency currency = Currency.EUR;

        // Act
        var response = await Client.GetAsync($"product/{currency}?pageStart={pageStart}&pageSize={pageSize}");

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);

        var contentAsString = await response.Content.ReadAsStringAsync();
        var products = JsonConvert.DeserializeObject<ProductWithCurrency[]>(contentAsString);
        
        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.Equal(int.Parse(pageSize), products.Length);
        Assert.All(products, p => Assert.Equal(currency, p.Currency));
    }
}