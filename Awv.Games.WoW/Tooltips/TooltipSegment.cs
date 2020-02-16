using Awv.Games.WoW.Tooltips.Interface;
using System.Collections.Generic;

namespace Awv.Games.WoW.Tooltips
{
    public class TooltipSegment : ITooltipSegment
    {
        public List<TooltipText> LeftTexts { get; set; } = new List<TooltipText>();
        public List<TooltipText> RightTexts { get; set; } = new List<TooltipText>();

        public IEnumerable<TooltipText> GetLeftTexts() => LeftTexts;

        public IEnumerable<TooltipText> GetRightTexts() => RightTexts;

        public void Level()
        {
            while (LeftTexts.Count < RightTexts.Count)
                LeftTexts.Add(TooltipText.Empty);
            while (RightTexts.Count < LeftTexts.Count)
                RightTexts.Add(TooltipText.Empty);
        }

        public void Append(ITooltipSegment segment, bool levelFirst = true)
        {
            if (levelFirst) Level();
            var lefts = segment.GetLeftTexts();
            var rights = segment.GetRightTexts();

            foreach (var left in lefts) LeftTexts.Add(left);
            foreach (var right in rights) RightTexts.Add(right);
        }
    }
}
