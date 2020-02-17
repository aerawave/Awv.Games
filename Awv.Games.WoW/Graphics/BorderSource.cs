using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Awv.Games.WoW.Graphics
{
    public class BorderSource : TileSource
    {
        /// <summary>
        /// Padding from the very edge of the graphics to where tooltip content should be rendered.
        /// </summary>
        public SizeF Padding { get; set; } = new SizeF(8f, 8f);
        /// <summary>
        /// Top left tile of the border
        /// </summary>
        public Image<Rgba32> TopLeft { get; private set; }
        /// <summary>
        /// Top right tile of the border
        /// </summary>
        public Image<Rgba32> TopRight { get; private set; }
        /// <summary>
        /// Bottom left tile of the border
        /// </summary>
        public Image<Rgba32> BottomLeft { get; private set; }
        /// <summary>
        /// Bottom right tile of the border
        /// </summary>
        public Image<Rgba32> BottomRight { get; private set; }

        /// <summary>
        /// Top tile of the border
        /// </summary>
        public Image<Rgba32> Top { get; private set; }
        /// <summary>
        /// Bottom tile of the border
        /// </summary>
        public Image<Rgba32> Bottom { get; private set; }
        /// <summary>
        /// Left tile of the border
        /// </summary>
        public Image<Rgba32> Left { get; private set; }
        /// <summary>
        /// Right tile of the border
        /// </summary>
        public Image<Rgba32> Right { get; private set; }

        /// <summary>
        /// <see cref="TileSource.Tile"/>s the source, then assigns this class's properties to tiles from the tileset.
        /// </summary>
        public override void Tile()
        {
            base.Tile();
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
