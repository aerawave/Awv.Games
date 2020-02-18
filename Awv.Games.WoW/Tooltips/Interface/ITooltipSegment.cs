using System;
using System.Collections.Generic;

namespace Awv.Games.WoW.Tooltips.Interface
{
    public interface ITooltipSegment : ICloneable
    {
        IEnumerable<TooltipText> GetLeftTexts();
        IEnumerable<TooltipText> GetRightTexts();
        ITooltipSegment Levelled();
        void Level();
    }
}
