using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System.Collections.Generic;
using System.IO;

namespace Awv.Games.WoW.Graphics
{
    /// <summary>
    /// A class to pull from a <see cref="Source"/> image as a tileset, into an array of <see cref="Tiles"/>. Note: Only works on one-dimensional tilesets.
    /// </summary>
    public class TileSource
    {
        /// <summary>
        /// A list of the individual tile images.
        /// </summary>
        protected List<Image<Rgba32>> Tiles { get; set; } = new List<Image<Rgba32>>();
        /// <summary>
        /// The size of each tile.
        /// </summary>
        public int TileSize { get; set; }
        /// <summary>
        /// The original source image.
        /// </summary>
        public Image<Rgba32> Source { get; set; }
        /// <summary>
        /// Loads the file at the given <paramref name="path"/> as an image, with the given <paramref name="tilesize"/>.
        /// </summary>
        /// <param name="path">Path to the image file</param>
        /// <param name="tilesize">Size of each tile</param>
        public virtual void Load(string path, int tilesize)
        {
            using var stream = File.OpenRead(path);
            Load(stream, tilesize);
        }
        /// <summary>
        /// Loads the <paramref name="stream"/> as an image, with the given <paramref name="tilesize"/>.
        /// </summary>
        /// <param name="stream">Stream of the image</param>
        /// <param name="tilesize">Size of each tile</param>
        public virtual void Load(Stream stream, int tilesize) => Load(Image.Load<Rgba32>(stream), tilesize);
        /// <summary>
        /// Loads the <paramref name="source"/> as the image, with the given <paramref name="tilesize"/>.
        /// </summary>
        /// <param name="source">The image</param>
        /// <param name="tilesize">Size of each tile</param>
        public virtual void Load(Image<Rgba32> source, int tilesize)
        {
            Source = source;
            TileSize = tilesize;
            Tile();
        }
        /// <summary>
        /// Runs the tiling process, clearing all old tiles out and adding them back again. Useful for overwriting tiles in inherited classes, then re-creating them here.
        /// </summary>
        public virtual void Tile()
        {
            foreach (var tile in Tiles) tile.Dispose();
            Tiles.Clear();



            for (var i = 0; i < Source.Width; i += TileSize)
            {
                Tiles.Add(Source.Clone(x => x.Crop(new Rectangle(i, 0, TileSize, TileSize))));
            }
        }
        /// <summary>
        /// Scales each of the <see cref="Tiles"/> up with the given <paramref name="scale"/> amount.
        /// </summary>
        /// <param name="scale">Amount to scale each tile by</param>
        public void Scale(float scale)
        {
            for (var i = 0; i < Tiles.Count;i++)
                Tiles[i].Mutate(x => x.Resize((int)(Tiles[i].Width * scale), (int)(Tiles[i].Height * scale)));
        }
    }
}
