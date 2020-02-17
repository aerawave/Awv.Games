using Awv.Games.Currency.Interface;
using System;
using System.Collections.Generic;

namespace Awv.Games.Currency
{
    /// <summary>
    /// A very simple implementation of the <see cref="ICurrency"/> interface that relies on a constant magnitude change between each unit of the currency. (for instance, 100 copper = 1 silver, 100 silver = 1 gold, 100 gold = 1 platinum)
    /// </summary>
    public class SimpleCurrency : ICurrency
    {
        /// <summary>
        /// The units of this currency.
        /// </summary>
        public List<CurrencyUnit> Units { get; set; } = new List<CurrencyUnit>();
        /// <summary>
        /// The threshhold which a unit of this currency most cross to be reduced to the next unit of the currency, if one is available.
        /// </summary>
        public long MagnitudeLevels { get; set; } = 100;


        public CurrencyCount GetCurrency(long amount)
        {
            var maxIndex = GetSimpleIndex(amount);
            var currentAmount = amount;

            var values = new CurrencyCount();

            for (var i = 0; i < maxIndex + 1; i++)
            {
                var magnitude = (int)Math.Pow(MagnitudeLevels, maxIndex - i);
                var count = currentAmount / magnitude;
                values.Add(new Count<CurrencyUnit>(Units[Units.Count - (Units.Count - maxIndex) - i], count));
                currentAmount -= count * magnitude;
            }

            values.Currency = this;
            return values;
        }

        /// <summary>
        /// Gets the maximum index of currency used by the given <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">Amount to check against</param>
        /// <returns>Maximum index of currency used by the given <paramref name="amount"/></returns>
        private int GetSimpleIndex(long amount)
        {
            var magnitudeIndex = 0;
            var currentAmount = amount;

            while (currentAmount >= MagnitudeLevels && magnitudeIndex < Units.Count)
            {
                currentAmount /= MagnitudeLevels;
                magnitudeIndex++;
            }

            return Math.Min(magnitudeIndex, Units.Count - 1);
        }

        /// <summary>
        /// Creates *and adds* a new unit of currency to this currency.
        /// </summary>
        /// <param name="name">The full name of the unity of currency</param>
        /// <param name="symbol">A shorthand symbol for the unit of currency</param>
        /// <returns></returns>
        public CurrencyUnit CreateUnit(string name, string symbol)
        {
            var unit = string.IsNullOrWhiteSpace(symbol) ? new CurrencyUnit(this, name) : new CurrencyUnit(this, name, symbol);
            Units.Add(unit);
            return unit;
        }
    }
}
