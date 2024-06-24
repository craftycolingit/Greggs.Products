using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Services.Interfaces
{
    public interface ICurrencyConverterService
    {
        /// <summary>
        /// Convert the price of a product to the specified currency
        /// </summary>
        /// <param name="price"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        decimal Convert(decimal price, Currency currency);
    }
}
