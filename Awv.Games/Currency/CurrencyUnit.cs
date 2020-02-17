using Awv.Games.Currency.Interface;

namespace Awv.Games.Currency
{
    /// <summary>
    /// A simple unit of currency.
    /// </summary>
    public class CurrencyUnit
    {
        /// <summary>
        /// The currency this derives from.
        /// </summary>
        public ICurrency Currency { get; set; }
        /// <summary>
        /// The full name of the unity of currency.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A shorthand symbol for the unit of currency.
        /// </summary>
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

        /// <summary>
        /// Returns the <see cref="Name"/> of this currency unit.
        /// </summary>
        /// <returns>The <see cref="Name"/> of this currency unit</returns>
        public override string ToString() => Name;
    }
}
