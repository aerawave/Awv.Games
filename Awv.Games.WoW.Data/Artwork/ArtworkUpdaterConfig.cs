using System.Collections.Generic;

namespace Awv.Games.WoW.Data.Artwork
{
    public class ArtworkUpdaterConfig
    {
        /// <summary>
        /// The quantity of items saved so far.
        /// </summary>
        public uint Count { get; set; }
        /// <summary>
        /// The quantity of items to convert before saving the JSON file.
        /// </summary>
        public int SaveInterval { get; set; } = 25;
        public List<ExceptionData> Errors { get; set; } = new List<ExceptionData>();
    }
}
