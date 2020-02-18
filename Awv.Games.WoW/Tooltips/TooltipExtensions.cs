using Awv.Games.WoW.Tooltips.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Games.WoW.Tooltips
{
    public static class TooltipExtensions
    {
        public static IEnumerable<TooltipLine> GetLines(this ITooltipSegment segment)
        {
            var levelled = segment.Levelled();
            return levelled.GetLeftTexts().Zip(levelled.GetRightTexts())
                .Select(texts => new TooltipLine(texts.First, texts.Second));
        }
        public static IEnumerable<TooltipLine> GetLines(this ITooltip tooltip)
            => tooltip.GetSegments().SelectMany(segment => segment.GetLines()).ToArray();

        public static string GetTooltipDisplayString(this TimeSpan span)
        {
            var cseconds = Math.Ceiling(span.TotalSeconds) % 60;

            if (span.TotalDays >= 1)
                return $"{span.Days} day{(span.Days > 1 ? "s" : "")}";
            else if (span.TotalHours >= 1)
                return $"{span.Hours} hour{(span.Hours > 1 ? "s" : "")}";
            else if (span.TotalMinutes >= 5)
                return $"{span.Minutes} minutes";
            else if (span.TotalMinutes >= 1)
                return $"{span.Minutes} minute{(span.Minutes > 1 ? "s" : "")}{(cseconds > 0 ? $" {cseconds} second{(cseconds > 1 ? "s" : "")}" : "")}";
            else if (span.TotalSeconds > 0)
                return $"{cseconds} second{(cseconds > 1 ? "s" : "")}";
            else
                return span.ToString();
        }
    }
}
