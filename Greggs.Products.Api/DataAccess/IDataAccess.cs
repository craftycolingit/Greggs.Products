using Greggs.Products.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Greggs.Products.Api.DataAccess;

public interface IDataAccess<out T>
{
    IEnumerable<T> List(int? pageStart, int? pageSize);
    Task<IEnumerable<Product>> ListAsync(int? pageStart, int? pageSize, Currency currency, bool latestProducts);
}