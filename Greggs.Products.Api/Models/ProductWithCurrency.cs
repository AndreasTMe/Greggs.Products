using System.Diagnostics.CodeAnalysis;
using Greggs.Products.Api.Helpers;

namespace Greggs.Products.Api.Models;

[ExcludeFromCodeCoverage]
public class ProductWithCurrency
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Currency Currency { get; set; }
}