using SixLabors.Fonts;
using SixLabors.Primitives;

namespace Awv.Games.WoW.Tooltips.Text.Interface
{
    public interface IRightText : ITooltipLine
    {
        ITooltipText GetRightText();
    }
}
