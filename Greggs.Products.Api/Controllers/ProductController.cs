using System.Collections.Generic;
using Greggs.Products.Api.Helpers;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;

    public ProductController(ILogger<ProductController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    [Route("")]
    public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5)
    {
        return _productService.List(pageStart, pageSize);
    }

    [HttpGet]
    [Route("{currency}")]
    public IEnumerable<ProductWithCurrency> Get(Currency currency = Currency.GBP, int pageStart = 0, int pageSize = 5)
    {
        return _productService.ListWithCurrency(currency, pageStart, pageSize);
    }
}