namespace Awv.Games.Currency.Interface
{
    /// <summary>
    /// Provides a framework for relatively rudamentary currencies. Good enough for most games.
    /// </summary>
    public interface ICurrency
    {
        /// <summary>
        /// Convert the given <paramref name="amount"/> of the lowest unit of currency to its most efficient form. For instance, in USD, 493 cents would be reduced to 4 dollars and 93 cents, given cents are the lowest unit.
        /// </summary>
        /// <param name="amount">Amount to be simplified</param>
        /// <returns>Simplified representation of the given <paramref name="amount"/> of currency</returns>
        CurrencyCount GetCurrency(long amount);
    }
}
