using System.Net.Http;
using Greggs.Products.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Greggs.Products.IntegrationTests;

public class IntegrationTests
{
    protected readonly HttpClient Client;
    
    public IntegrationTests()
    {
        var appFactory = new WebApplicationFactory<Startup>();
        Client = appFactory.CreateClient();
    }
}