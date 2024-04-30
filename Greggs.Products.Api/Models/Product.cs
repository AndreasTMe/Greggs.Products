using System.Diagnostics.CodeAnalysis;

namespace Greggs.Products.Api.Models;

[ExcludeFromCodeCoverage]
public class Product
{
    public string Name { get; set; }
    public decimal PriceInPounds { get; set; }
}