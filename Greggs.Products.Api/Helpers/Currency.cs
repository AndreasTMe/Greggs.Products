using System.Text.Json.Serialization;

namespace Greggs.Products.Api.Helpers;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Currency
{
    GBP = 0,
    EUR
}