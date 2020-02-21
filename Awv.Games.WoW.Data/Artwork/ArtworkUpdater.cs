using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using Warcraft.BLP;

namespace Awv.Games.WoW.Data.Artwork
{
    public class ArtworkUpdater
    {
        /// <summary>
        /// Where to store extremely basic data about the <see cref="ArtworkUpdater"/> so it can resume where it last left off, given the quantity of files you will likely be working with.
        /// </summary>
        public string UpdaterJson { get; set; }
        /// <summary>
        /// Whether or not to restart from a <see cref="Count"/> of 0 when <see cref="Start"/> is called.
        /// </summary>
        public bool Restart { get; set; }
        /// <summary>
        /// The directory from which BLP images will be imported.
        /// </summary>
        public string InputBlpDirectory { get; set; }
        /// <summary>
        /// The directory into which PNG images will be exported.
        /// </summary>
        public string OutputPngDirectory { get; set; }
        public ArtworkUpdaterConfig Config { get; set; }

        /// <summary>
        /// Instantiates a new ArtworkUpdater with the given input and output directories.
        /// </summary>
        /// <param name="updaterJson">Where to store extremely basic data about the <see cref="ArtworkUpdater"/> so it can resume where it last left off, given the quantity of files you will likely be working with</param>
        /// <param name="inputBlpDirectory">The directory from which BLP images will be imported</param>
        /// <param name="outputPngDirectory">The directory into which PNG images will be exported</param>
        public ArtworkUpdater(string updaterJson, string inputBlpDirectory, string outputPngDirectory)
        {
            if (string.IsNullOrWhiteSpace(updaterJson)) throw new ArgumentNullException(nameof(updaterJson), "A JSON config file path is required to store progress.");
            if (string.IsNullOrWhiteSpace(inputBlpDirectory)) throw new ArgumentNullException(nameof(inputBlpDirectory), "The input BLP directory is required.");
            if (string.IsNullOrWhiteSpace(outputPngDirectory)) throw new ArgumentNullException(nameof(outputPngDirectory), "The output PNG directory is required.");
            UpdaterJson = updaterJson;
            InputBlpDirectory = inputBlpDirectory;
            OutputPngDirectory = outputPngDirectory;
        }

        public void Start()
        {
            if (File.Exists(UpdaterJson) && !Restart)
                Config = JsonConvert.DeserializeObject<ArtworkUpdaterConfig>(File.ReadAllText(UpdaterJson));
            else
                Config = new ArtworkUpdaterConfig();

            var blpFiles = Directory.GetFiles(InputBlpDirectory, "*.blp", SearchOption.AllDirectories).Skip(0);

            var maximum = (uint)blpFiles.Count();
            var current = Config.Count;
            blpFiles = blpFiles.Skip((int)current);

            var percentPrecision = 2;
            var percentLength = 4 + (percentPrecision + (percentPrecision > 0 ? 1 : 0));
            var maxLength = maximum.ToString().Length;

            var lead = "Converting file ";

            Console.Write(lead);

            foreach(var blpPath in blpFiles)
            {
                Console.SetCursorPosition(lead.Length, 0);
                Console.WriteLine($"{(++current).ToString().PadLeft(maxLength)} / {maximum.ToString().PadLeft(maxLength)} ({((double)current / maximum).ToString($"P{percentPrecision}").PadLeft(percentLength)})");

                try
                {
                    var pngPath = Path.ChangeExtension(blpPath.Replace(InputBlpDirectory, OutputPngDirectory), ".png");
                    var pngDir = Path.GetDirectoryName(pngPath);

                    if (!Directory.Exists(pngDir))
                        Directory.CreateDirectory(pngDir);

                    using var blpFile = File.Open(blpPath, FileMode.Open);
                    using var pngFile = File.Open(pngPath, FileMode.Create);
                    using var reader = new BinaryReader(blpFile);

                    var blp = new BLP(reader.ReadBytes((int)blpFile.Length));
                    var png = blp.GetBestMipMap(Math.Max(blp.Header.Resolution.X, blp.Header.Resolution.Y));

                    png.SaveAsPng(pngFile);
                }
                catch(Exception exception)
                {
                    var exists = Config.Errors.Any(error => error.Index == current);

                    if (!exists)
                        Config.Errors.Add(new ExceptionData(current, blpPath, exception));
                }

                if (current % Config.SaveInterval == 0)
                {
                    Config.Count = current;
                    File.WriteAllText(UpdaterJson, JsonConvert.SerializeObject(Config));
                }
            }
        }
    }
}
