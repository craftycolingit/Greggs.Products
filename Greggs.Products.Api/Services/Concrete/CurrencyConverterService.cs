using Greggs.Products.Api.Services.Interfaces;
using Greggs.Products.Api.Models;
using System;

namespace Greggs.Products.Api.Services.Concrete
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        /// <summary>
        /// Convert the price of a product to the specified currency
        /// </summary>
        /// <param name="price"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public decimal Convert(decimal price, Currency currency)
        {
            return Math.Round(price * (int)currency / 100, 2, MidpointRounding.AwayFromZero);
        }

    }
}
