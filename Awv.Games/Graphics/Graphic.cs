using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace Awv.Games.Graphics
{
    public class Graphic : IGraphic
    {
        /// <summary>
        /// The image referenced by this graphic. Will auto populate from <see cref="PathReference"/> if that is provided and this is not.
        /// </summary>
        public Image<Rgba32> ImageReference { get; set; }
        /// <summary>
        /// The path at which an image can be located to read for this graphic, and put into the <see cref="ImageReference"/> value.
        /// </summary>
        public string PathReference { get; set; }

        public Graphic(Image<Rgba32> imageReference)
        {
            ImageReference = imageReference;
        }

        public Graphic(string pathReference)
        {
            PathReference = pathReference;
        }

        /// <summary>
        /// Updates and returns the <see cref="ImageReference"/> if a <see cref="PathReference"/> is provided.
        /// </summary>
        /// <returns></returns>
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
