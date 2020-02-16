using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Awv.Games.WoW.Graphics
{
    public class BorderSource : TileSource
    {
        public SizeF Padding { get; set; } = new SizeF(8f, 8f);
        public Image<Rgba32> TopLeft { get; private set; }
        public Image<Rgba32> TopRight { get; private set; }
        public Image<Rgba32> BottomLeft { get; private set; }
        public Image<Rgba32> BottomRight { get; private set; }

        public Image<Rgba32> Top { get; private set; }
        public Image<Rgba32> Bottom { get; private set; }
        public Image<Rgba32> Left { get; private set; }
        public Image<Rgba32> Right { get; private set; }

        public override void Retile()
        {
            base.Retile();
            Padding = new SizeF((float)TileSize * .5f, (float)TileSize * .5f);

            Tiles[2].Mutate(x => x.Rotate(90f));
            Tiles[3].Mutate(x => x.Rotate(90f));

            TopLeft = Tiles[4];
            TopRight = Tiles[5];
            BottomLeft = Tiles[6];
            BottomRight = Tiles[7];

            Top = Tiles[2];
            Bottom = Tiles[3];
            Left = Tiles[0];
            Right = Tiles[1];
        }
    }
}
