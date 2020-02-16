using System.Collections.Generic;

namespace Awv.Games.WoW.Tooltips.Interface
{
    public interface ITooltipSegment
    {
        IEnumerable<TooltipText> GetLeftTexts();
        IEnumerable<TooltipText> GetRightTexts();
        void Level();
    }
}
