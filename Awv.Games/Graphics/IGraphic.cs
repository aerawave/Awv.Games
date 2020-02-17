using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.Graphics
{
    /// <summary>
    /// An interface for requesting an graphic.
    /// </summary>
    public interface IGraphic
    {
        Image<Rgba32> GetImage();
    }
}
