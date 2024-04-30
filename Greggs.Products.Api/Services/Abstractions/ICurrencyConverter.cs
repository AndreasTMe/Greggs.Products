using Greggs.Products.Api.Helpers;

namespace Greggs.Products.Api.Services.Abstractions;

public interface ICurrencyConverter
{
    decimal GetExchangeRateFromPounds(Currency to);
}