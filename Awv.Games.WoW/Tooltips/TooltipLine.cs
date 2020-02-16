namespace Awv.Games.WoW.Tooltips
{
    public class TooltipLine
    {
        public TooltipText LeftText { get; set; } = TooltipText.Empty;
        public TooltipText RightText { get; set; } = TooltipText.Empty;

        public TooltipLine() { }

        public TooltipLine(TooltipText left, TooltipText right)
        {
            LeftText = left;
            RightText = right;
        }
    }
}
