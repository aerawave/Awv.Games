using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.WoW.Graphics
{
    public class CurrencySource : TileSource
    {
        public Image<Rgba32> Gold { get; set; }
        public Image<Rgba32> Silver { get; set; }
        public Image<Rgba32> Copper { get; set; }

        public override void Retile()
        {
            base.Retile();

            Gold = Tiles[0];
            Silver = Tiles[1];
            Copper = Tiles[2];
        }
    }
}
