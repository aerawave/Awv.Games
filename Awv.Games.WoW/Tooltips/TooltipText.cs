using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.WoW.Tooltips
{
    public struct TooltipText
    {
        public string Text { get; set; }
        public Rgba32 Color { get; set; }
        public TooltipTextType Type { get; set; }

        public TooltipText(string text) : this() { Text = text; }
        public TooltipText(string text, TooltipTextType type) : this (text) { Type = type; }
        public TooltipText(Rgba32 color) : this() { Color = color; }
        public TooltipText(Rgba32 color, TooltipTextType type) : this(color) { Type = type; }
        public TooltipText(string text, Rgba32 color) : this(text) { Color = color; }
        public TooltipText(string text, Rgba32 color, TooltipTextType type) : this(text, color) { Type = type; }

        public static implicit operator TooltipText(string text) => new TooltipText(text, TooltipColors.Common);

        public static readonly TooltipText Empty = new TooltipText("", TooltipColors.Common);

        public override string ToString() => Text;
    }
}
