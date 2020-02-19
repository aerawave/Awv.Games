namespace Awv.Games.WoW.Tooltips.Text.Interface
{
    public interface ICurrencyLine : ITooltipLine
    {
        public int GetGold();
        public int GetSilver();
        public int GetCopper();
    }
}
