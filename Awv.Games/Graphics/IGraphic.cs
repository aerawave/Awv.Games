using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.Graphics
{
    public interface IGraphic
    {
        Image<Rgba32> GetImage();
    }
}
