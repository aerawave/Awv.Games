using Awv.Games.Currency;

namespace Awv.Games.WoW
{
    public class WoWCurrency : SimpleCurrency
    {
        public WoWCurrency()
        {
            CreateUnit("Copper", "c");
            CreateUnit("Silver", "s");
            CreateUnit("Gold", "g");
        }
    }
}
