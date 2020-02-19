using Awv.Games.WoW.Tooltips.Text.Interface;
using SixLabors.Fonts;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace Awv.Games.WoW.Tooltips
{
    public struct TooltipText : ITooltipText
    {
        #region Properties
        public string Text { get; set; }
        public Rgba32 Color { get; set; }
        #endregion
        #region ITooltipText Accessors
        public string GetText() => Text;
        public Rgba32 GetColor() => Color;
        public SizeF Measure(RendererOptions renderer) => TextMeasurer.Measure(Text, renderer);
        #endregion
        #region Constructors
        public TooltipText(string text) : this() { Text = text; }
        public TooltipText(Rgba32 color) : this() { Color = color; }
        public TooltipText(string text, Rgba32 color) : this(text) { Color = color; }
        #endregion
        #region Other Methods
        public static implicit operator TooltipText(string text) => new TooltipText(text, TooltipColors.Common);
        public static implicit operator TooltipLine(TooltipText text) => new TooltipLine(text, null);

        public static readonly TooltipText Empty = new TooltipText("", TooltipColors.Common);
        public override string ToString() => Text;
        #endregion
    }
}
