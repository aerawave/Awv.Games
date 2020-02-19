using Awv.Games.WoW.Tooltips.Text.Interface;
using SixLabors.Fonts;
using SixLabors.Primitives;
using System;
using System.Linq;

namespace Awv.Games.WoW.Tooltips
{
    public static class TooltipExtensions
    {
        public static SizeF Measure(this ITooltipLine line, RendererOptions renderer)
        {
            var sizeLeft = (line as ILeftText)?.Measure(renderer) ?? SizeF.Empty;
            var sizeRight = (line as IRightText)?.Measure(renderer) ?? SizeF.Empty;
            var sizeParagraph = (line as IParagraphLine)?.Measure(renderer) ?? SizeF.Empty;
            var sizes = new SizeF[] {
                (line as ILeftText)?.Measure(renderer) ?? SizeF.Empty,
                (line as IRightText)?.Measure(renderer) ?? SizeF.Empty,
                (line as IParagraphLine)?.Measure(renderer) ?? SizeF.Empty
            };
            return new SizeF(sizes.Sum(size => size.Width), sizes.Max(size => size.Height));
        }

        public static SizeF Measure(this ILeftText leftText, RendererOptions renderer)
            => TextMeasurer.Measure(leftText.GetLeftText().GetText(), renderer);
        public static SizeF Measure(this IRightText rightText, RendererOptions renderer)
            => TextMeasurer.Measure(rightText.GetRightText().GetText(), renderer);
        public static SizeF Measure(this IParagraphLine paragraph, RendererOptions renderer)
            => TextMeasurer.Measure(paragraph.GetParagraph().GetText(), renderer);

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
