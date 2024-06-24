using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services.Interfaces;

namespace Greggs.Products.Api.DataAccess;

/// <summary>
/// DISCLAIMER: This is only here to help enable the purpose of this exercise, this doesn't reflect the way we work!
/// </summary>
public class ProductAccess : IDataAccess<Product>
{
    private readonly ICurrencyConverterService _currencyConverterService;

    public ProductAccess(ICurrencyConverterService currencyConverterService)
    {
        _currencyConverterService = currencyConverterService;
    }

    private static readonly IEnumerable<Product> ProductDatabase = new List<Product>()
    {
        new() { Name = "Sausage Roll", Price = 1m },
        new() { Name = "Vegan Sausage Roll", Price = 1.1m },
        new() { Name = "Steak Bake", Price = 1.2m },
        new() { Name = "Yum Yum", Price = 0.7m },
        new() { Name = "Pink Jammie", Price = 0.5m },
        new() { Name = "Mexican Baguette", Price = 2.1m },
        new() { Name = "Bacon Sandwich", Price = 1.95m },
        new() { Name = "Coca Cola", Price = 1.2m }
    };

    public IEnumerable<Product> List(int? pageStart, int? pageSize)
    {
        var queryable = ProductDatabase.AsQueryable();

        if (pageStart.HasValue)
            queryable = queryable.Skip(pageStart.Value);

        if (pageSize.HasValue)
            queryable = queryable.Take(pageSize.Value);

        return queryable.ToList();
    }

    /// <summary>
    /// Get a list of products, optionally paginated, optionally converted to a different currency, optionally sorted by latest products
    /// </summary>
    /// <param name="pageStart"></param>
    /// <param name="pageSize"></param>
    /// <param name="currency"></param>
    /// <param name="latestProducts"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Product>> ListAsync(int? pageStart, int? pageSize, Currency currency, bool latestProducts)
    {
        // create a queryable from the product database
        var queryable = ProductDatabase.AsQueryable();

        // apply pagination and sorting
        queryable = ApplyPaginationAndSorting(pageStart, pageSize, latestProducts, queryable);

        // return the products
        return await Task.FromResult(queryable.Select(product => new Product
        {
            Name = product.Name,
            Price = _currencyConverterService.Convert(product.Price, currency)
        }).ToArray());
    }

    /// <summary>
    /// Apply pagination and sorting to the queryable, depending on latestProducts
    /// </summary>
    /// <param name="pageStart">the start of the page</param>
    /// <param name="pageSize">the size of the page</param>
    /// <param name="latestProducts">latest products true or false</param>
    /// <param name="queryable">the query</param>
    /// <returns>Pagination and sorting for the queryable</returns>
    private static IQueryable<Product> ApplyPaginationAndSorting(int? pageStart, int? pageSize, bool latestProducts, IQueryable<Product> queryable)
    {
        if (latestProducts)
        {
            // for the purposes of this exercise, we'll show all the latest products irrespective of the pageStart and pageSize
            // if we had the added/created date, we could order by that
            queryable = queryable.Skip(0).Take(ProductDatabase.Count()).OrderBy(x => x.Name);
        }
        else
        {
            // if pageStart has a value, skip the number of products
            if (pageStart.HasValue)
                queryable = queryable.Skip(pageStart.Value);

            // if pageSize has a value, take the number of products
            if (pageSize.HasValue)
            {
                queryable = queryable.Take(pageSize.Value);
            }
        }

        return queryable;
    }
}