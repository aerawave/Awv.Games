using Awv.Games.Currency.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Games.Currency
{
    public class CurrencyCount : List<Count<CurrencyUnit>>
    {
        public ICurrency Currency { get; set; }

        public bool HasValue => Count > 0 && this.Any(count => count.Quantity > 0);

        public override string ToString() => string.Join(" ", this.Select(count => $"{count.Quantity}{count.Value.Symbol}"));

        public void SetAmount(long amount)
        {
            var count = Currency.GetCurrency(amount);

            Clear();

            count.ForEach(ct => Add(ct));
        }
    }
}
