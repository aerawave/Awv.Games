using Awv.Games.WoW.Tooltips.Text.Interface;
using SixLabors.Fonts;
using SixLabors.Primitives;

namespace Awv.Games.WoW.Tooltips.Text
{
    public struct CurrencyLine : ILeftText, ICurrencyLine
    {
        private TooltipText CurrencyText { get; set; }
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Copper { get; set; }
        public CurrencyLine(string currencyText)
            : this()
        {
            CurrencyText = currencyText;
        }

        public CurrencyLine(int gold, int silver, int copper)
            : this()
        {
            Gold = gold;
            Silver = silver;
            Copper = copper;
        }

        public ITooltipText GetLeftText() => CurrencyText;
        public SizeF Measure(RendererOptions renderer) => TextMeasurer.Measure(GetLeftText().GetText(), renderer);
        public SizeF MeasureLeft(RendererOptions renderer) => Measure(renderer);
        public int GetGold() => Gold;
        public int GetSilver() => Silver;
        public int GetCopper() => Copper;
    }
}
