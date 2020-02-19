using Awv.Games.WoW.Tooltips.Text.Interface;
using System.Collections.Generic;

namespace Awv.Games.WoW.Tooltips.Interface
{
    public interface ITooltipSection
    {
        IEnumerable<ITooltipLine> GetLines();
    }
}
