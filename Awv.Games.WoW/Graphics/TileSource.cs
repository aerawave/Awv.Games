using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System.Collections.Generic;
using System.IO;

namespace Awv.Games.WoW.Graphics
{
    public class TileSource
    {
        protected List<Image<Rgba32>> Tiles { get; set; } = new List<Image<Rgba32>>();
        public int TileSize { get; set; }
        public Image<Rgba32> Source { get; set; }

        public virtual void Load(string path, int tilesize)
        {
            using var stream = File.OpenRead(path);
            Load(stream, tilesize);
        }
        public virtual void Load(Stream stream, int tilesize) => Load(Image.Load<Rgba32>(stream), tilesize);
        public virtual void Load(Image<Rgba32> source, int tilesize)
        {
            Source = source;
            TileSize = tilesize;
            Retile();
        }

        public virtual void Retile()
        {
            foreach (var tile in Tiles) tile.Dispose();
            Tiles.Clear();



            for (var i = 0; i < Source.Width; i += TileSize)
            {
                Tiles.Add(Source.Clone(x => x.Crop(new Rectangle(i, 0, TileSize, TileSize))));
            }
        }

        public void Rescale(float scale)
        {
            for (var i = 0; i < Tiles.Count;i++)
                Tiles[i].Mutate(x => x.Resize((int)(Tiles[i].Width * scale), (int)(Tiles[i].Height * scale)));
        }
    }
}
