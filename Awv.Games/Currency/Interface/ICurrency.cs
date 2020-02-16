namespace Awv.Games.Currency.Interface
{
    public interface ICurrency
    {
        CurrencyCount GetCurrency(long amount);
    }
}
