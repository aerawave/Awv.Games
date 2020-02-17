using Awv.Games.WoW.Tooltips.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Games.WoW.Tooltips
{
    public static class TooltipExtensions
    {
        public static TooltipSegment Normalize(this ITooltipSegment segment)
        {
            var target = new TooltipSegment();

            segment.GetLeftTexts().ToList().ForEach(text => target.LeftTexts.Add(text));
            segment.GetRightTexts().ToList().ForEach(text => target.RightTexts.Add(text));

            var count = target.LeftTexts.Count - target.RightTexts.Count;

            if (count != 0)
            {
                var lean = count / Math.Abs(count);
                count = Math.Abs(count);

                switch (lean)
                {
                    case -1: // left needs compensation
                        for (var i = 0; i < count; i++)
                            target.LeftTexts.Add(TooltipText.Empty);
                        break;
                    case 1: // right needs compensation
                        for (var i = 0; i < count; i++)
                            target.RightTexts.Add(TooltipText.Empty);
                        break;
                }
            }

            return target;
        }

        public static TooltipSegment ToSegment(this ITooltip tooltip)
        {
            var target = new TooltipSegment();
            var segments = tooltip.GetSegments();

            foreach (var segment in segments)
            {
                var segmentNormalized = segment.Normalize();
                segmentNormalized.GetLeftTexts().ToList().ForEach(text => target.LeftTexts.Add(text));
                segmentNormalized.GetRightTexts().ToList().ForEach(text => target.RightTexts.Add(text));
            }

            return target;
        }

        public static IEnumerable<TooltipLine> GetLines(this ITooltip tooltip)
        {
            var lines = new List<TooltipLine>();

            var segments = tooltip.GetSegments();

            segments.ToList().ForEach(unnormalizedSegment =>
            {
                var segment = unnormalizedSegment.Normalize();
                var leftTexts = segment.GetLeftTexts().ToArray();
                var rightTexts = segment.GetRightTexts().ToArray();

                for (var i = 0; i < leftTexts.Length; i++)
                {
                    lines.Add(new TooltipLine(leftTexts[i], rightTexts[i]));
                }
            });

            return lines;
        }

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
