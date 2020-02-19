using Awv.Games.WoW.Tooltips.Text.Interface;
using SixLabors.Fonts;
using SixLabors.Primitives;

namespace Awv.Games.WoW.Tooltips
{
    public struct RightText : IRightText
    {
        public ITooltipText Text { get; set; }
        public RightText(ITooltipText text)
        {
            Text = text;
        }
        public RightText(TooltipText text)
            : this(text as ITooltipText) { }
        public ITooltipText GetRightText() => Text;

        public LeftText ToLeft() => new LeftText(Text);
        public TooltipLine Prepend(LeftText left) => new TooltipLine(left.GetLeftText(), GetRightText());
        public static TooltipLine operator +(RightText right, LeftText left) => right.Prepend(left);

        public override string ToString() => $"-->{Text}";
    }
}
