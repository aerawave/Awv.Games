using System.Collections.Generic;

namespace Awv.Games.WoW.Tooltips.Interface
{
    public interface ITooltip
    {
        TooltipText GetTitle();
        IEnumerable<ITooltipSegment> GetSegments();
    }
}
