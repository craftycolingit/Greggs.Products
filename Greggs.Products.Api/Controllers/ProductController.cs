using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private static readonly string[] Products = new[]
    {
        "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    };

    private readonly ILogger<ProductController> _logger;
    private readonly IDataAccess<Product> _dataAccess;

    public ProductController(ILogger<ProductController> logger, IDataAccess<Product> dataAccess)
    {
        _logger = logger;
        _dataAccess = dataAccess;
    }

    // Hide this endpoint from the swagger documentation as its not needed
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet]
    public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5)
    {
        if (pageSize > Products.Length)
            pageSize = Products.Length;

        var rng = new Random();
        return Enumerable.Range(1, pageSize).Select(index => new Product
        {
            Price = rng.Next(0, 10),
            Name = Products[rng.Next(Products.Length)]
        })
            .ToArray();
    }

    // Get a list of products, optionally paginated, optionally converted to a different currency, optionally sorted by latest products
    [HttpGet("GetProducts")]
    public async Task<ActionResult> GetProducts(int pageStart = 0, int pageSize = 5, Currency currency = Currency.GBP, bool latestProducts = true)
    {
        try
        {
            var products = await _dataAccess.ListAsync(pageStart, pageSize, currency, latestProducts);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to get products");
            return StatusCode(500, "An error occurred while trying to get products");
        }
    }
}