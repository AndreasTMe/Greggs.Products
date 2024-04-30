using System;
using Greggs.Products.Api.Helpers;
using Greggs.Products.Api.Services;
using Xunit;

namespace Greggs.Products.UnitTests.Services;

public class StaticCurrencyConverterTests
{
    private readonly StaticCurrencyConverter _currencyConverter;

    public StaticCurrencyConverterTests()
    {
        _currencyConverter = new StaticCurrencyConverter();
    }

    [Fact]
    public void GetExchangeRateFromPounds_WhenGBPRequested_Return1()
    {
        // Arrange
        const Currency currency = Currency.GBP;

        // Act
        var exchangeRate = _currencyConverter.GetExchangeRateFromPounds(currency);

        // Assert
        Assert.Equal(1, exchangeRate);
    }
    
    [Fact]
    public void GetExchangeRateFromPounds_WhenEURRequested_Return1_11()
    {
        // Arrange
        const Currency currency = Currency.EUR;

        // Act
        var exchangeRate = _currencyConverter.GetExchangeRateFromPounds(currency);

        // Assert
        Assert.Equal(1.11m, exchangeRate);
    }
    
    [Fact]
    public void GetExchangeRateFromPounds_WhenNotSupportedCurrencyRequested_Throw()
    {
        // Arrange
        const Currency currency = (Currency) 99999;

        // Act
        // Assert
        Assert.Throws<NotSupportedException>(() => _currencyConverter.GetExchangeRateFromPounds(currency));
    }
}