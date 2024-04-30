using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Helpers;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services.Abstractions;

namespace Greggs.Products.Api.Services;

public class ProductService : IProductService
{
    private readonly IDataAccess<Product> _dataAccess;
    private readonly ICurrencyConverter _currencyConverter;

    public ProductService(IDataAccess<Product> dataAccess, ICurrencyConverter currencyConverter)
    {
        _dataAccess = dataAccess;
        _currencyConverter = currencyConverter;
    }

    public IEnumerable<Product> List(int? pageStart, int? pageSize)
    {
        return _dataAccess.List(pageStart, pageSize);
    }

    public IEnumerable<ProductWithCurrency> ListWithCurrency(Currency currency, int? pageStart, int? pageSize)
    {
        var products = _dataAccess
            .List(pageStart, pageSize)
            .ToArray();

        if (products.Length == 0)
        {
            return Enumerable.Empty<ProductWithCurrency>();
        }

        if (currency == Currency.GBP)
        {
            return products.Select(p => new ProductWithCurrency
            {
                Name = p.Name,
                Price = p.PriceInPounds,
                Currency = Currency.GBP
            });
        }

        return products.Select(p =>
        {
            var price = _currencyConverter.GetExchangeRateFromPounds(currency) * p.PriceInPounds;

            return new ProductWithCurrency
            {
                Name = p.Name,
                Price = Math.Round(price, 2, MidpointRounding.AwayFromZero),
                Currency = currency
            };
        });
    }
}