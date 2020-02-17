using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.WoW.Graphics
{
    public class CurrencySource : TileSource
    {
        /// <summary>
        /// Gold icon
        /// </summary>
        public Image<Rgba32> Gold { get; set; }
        /// <summary>
        /// Silver icon
        /// </summary>
        public Image<Rgba32> Silver { get; set; }
        /// <summary>
        /// Copper icon
        /// </summary>
        public Image<Rgba32> Copper { get; set; }

        /// <summary>
        /// <see cref="TileSource.Tile"/>s the source, then assigns this class's properties to tiles from the tileset.
        /// </summary>
        public override void Tile()
        {
            base.Tile();

            Gold = Tiles[0];
            Silver = Tiles[1];
            Copper = Tiles[2];
        }
    }
}
