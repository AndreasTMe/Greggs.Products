using System;
using Greggs.Products.Api.Helpers;
using Greggs.Products.Api.Services.Abstractions;

namespace Greggs.Products.Api.Services;

public class StaticCurrencyConverter : ICurrencyConverter
{
    public decimal GetExchangeRateFromPounds(Currency to)
    {
        return to switch
        {
            Currency.GBP => 1,
            Currency.EUR => 1.11m,
            _ => throw new NotSupportedException("Not supported currency conversion.")
        };
    }
}