using System.Collections.Generic;
using Greggs.Products.Api.Helpers;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Services.Abstractions;

public interface IProductService
{
    IEnumerable<Product> List(int? pageStart, int? pageSize);
    IEnumerable<ProductWithCurrency> ListWithCurrency(Currency currency, int? pageStart, int? pageSize);
}