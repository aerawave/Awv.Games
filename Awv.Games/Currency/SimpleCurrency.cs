using Awv.Games.Currency.Interface;
using System;
using System.Collections.Generic;

namespace Awv.Games.Currency
{
    public class SimpleCurrency : ICurrency
    {
        public List<CurrencyUnit> Units { get; set; } = new List<CurrencyUnit>();
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

        public CurrencyUnit CreateUnit(string name, string symbol)
        {
            var unit = string.IsNullOrWhiteSpace(symbol) ? new CurrencyUnit(this, name) : new CurrencyUnit(this, name, symbol);
            Units.Add(unit);
            return unit;
        }
    }
}
