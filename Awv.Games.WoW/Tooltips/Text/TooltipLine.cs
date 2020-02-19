using Awv.Games.WoW.Tooltips.Text.Interface;
using SixLabors.Fonts;
using SixLabors.Primitives;

namespace Awv.Games.WoW.Tooltips
{
    public struct TooltipLine : ILeftText, IRightText
    {
        #region Properties
        public ITooltipText LeftText { get; set; }
        public ITooltipText RightText { get; set; }
        #endregion
        #region ITooltipLine Accessors
        public ITooltipText GetLeftText() => LeftText;
        public ITooltipText GetRightText() => RightText;
        #endregion
        #region Methods
        public TooltipLine(ITooltipText left, ITooltipText right)
        {
            LeftText = left;
            RightText = right;
        }
        public override string ToString() => $"{LeftText}<---->{RightText}";
        #endregion
    }
}
