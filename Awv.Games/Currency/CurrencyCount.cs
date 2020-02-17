using Awv.Games.Currency.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Games.Currency
{
    public class CurrencyCount : List<Count<CurrencyUnit>>
    {
        /// <summary>
        /// Currency this count is based on.
        /// </summary>
        public ICurrency Currency { get; set; }
        /// <summary>
        /// Whether or not there are any counts greater than zero in this currency count.
        /// </summary>
        public bool HasValue => Count > 0 && this.Any(count => count.Quantity > 0);
        /// <summary>
        /// A basic representation of this currency count.
        /// </summary>
        /// <returns>A basic representation of this currency count</returns>
        public override string ToString() => string.Join(" ", this.Select(count => $"{count.Quantity}{count.Value.Symbol}"));
        /// <summary>
        /// Changes the values of this currency count inplace, so as to not instantiate a new object.
        /// </summary>
        /// <param name="amount">Amount of currency to be simplified</param>
        public void SetAmount(long amount)
        {
            var count = Currency.GetCurrency(amount);

            Clear();

            count.ForEach(ct => Add(ct));
        }
    }
}
