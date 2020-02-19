using Awv.Games.WoW.Tooltips.Text.Interface;
using SixLabors.Fonts;
using SixLabors.Primitives;

namespace Awv.Games.WoW.Tooltips
{
    public struct LeftText : ILeftText
    {
        public ITooltipText Text { get; set; }
        public LeftText(ITooltipText text)
        {
            Text = text;
        }
        public LeftText(TooltipText text)
            : this(text as ITooltipText) { }
        public ITooltipText GetLeftText() => Text;
        public RightText ToRight() => new RightText(Text);
        public TooltipLine Append(RightText right) => new TooltipLine(GetLeftText(), right.GetRightText());
        public SizeF Measure(RendererOptions renderer) => TextMeasurer.Measure(GetLeftText().GetText(), renderer);

        public static TooltipLine operator +(LeftText left, RightText right) => left.Append(right);

        public override string ToString() => $"{Text}<--";
    }
}
