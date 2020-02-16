using Awv.Games.Currency.Interface;

namespace Awv.Games.Currency
{
    public class CurrencyUnit
    {
        public ICurrency Currency { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }

        public CurrencyUnit(ICurrency currency, string name, string symbol)
        {
            Currency = currency;
            Name = name;
            Symbol = symbol;
        }

        public CurrencyUnit(ICurrency currency, string name)
            : this(currency, name, name)
        {
        }

        public override string ToString() => Name;
    }
}
