using Awv.Games.WoW.Tooltips.Interface;
using Awv.Games.WoW.Tooltips.Text.Interface;
using System.Collections.Generic;

namespace Awv.Games.WoW.Tooltips
{
    public class TooltipSection : ITooltipSection
    {
        #region Properties
        public List<ITooltipLine> Lines { get; set; } = new List<ITooltipLine>();
        #endregion
        #region ITooltipSegment Accessors
        public IEnumerable<ITooltipLine> GetLines() => Lines;
        #endregion
        #region Methods
        public void Append(ITooltipSection segment)
        {
            var lines = segment.GetLines();

            foreach (var line in lines)
                Lines.Add(line);
        }
        #endregion
    }
}
