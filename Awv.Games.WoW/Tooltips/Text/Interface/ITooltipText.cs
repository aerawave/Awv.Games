using SixLabors.Fonts;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace Awv.Games.WoW.Tooltips.Text.Interface
{
    public interface ITooltipText
    {
        string GetText();
        Rgba32 GetColor();
        SizeF Measure(RendererOptions renderer);
    }
}
