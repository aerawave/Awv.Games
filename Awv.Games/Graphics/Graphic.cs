using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace Awv.Games.Graphics
{
    public class Graphic : IGraphic
    {
        public Image<Rgba32> ImageReference { get; set; }
        public string PathReference { get; set; }

        public Graphic(Image<Rgba32> imageReference)
        {
            ImageReference = imageReference;
        }

        public Graphic(string pathReference)
        {
            PathReference = pathReference;
        }

        public Image<Rgba32> UpdateImage()
        {
            if (string.IsNullOrWhiteSpace(PathReference))
                throw new ArgumentNullException(nameof(PathReference));
            ImageReference = Image.Load<Rgba32>(PathReference);
            return ImageReference;
        }

        public Image<Rgba32> GetImage() => !string.IsNullOrEmpty(PathReference) ? UpdateImage() : ImageReference;
    }
}
