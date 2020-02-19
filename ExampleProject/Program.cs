using Awv.Games.WoW.Data;
using Awv.Games.WoW.Data.Artwork;
using Awv.Games.WoW.Data.Items;
using System;
using System.IO;
using System.Text;

namespace ExampleProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CustomWoWItems();
        }

        /// <summary>
        /// An example of how to use the <see cref="ArtworkUpdater"/>.
        /// 
        /// This will run through all files in a given directory (the "blpDirectory" variable below) and convert every *.blp file to a *.png file, and save it to the output directory (the "pngDirectory" variable below).
        /// 
        /// If you're not sure how to get the BLP files / directory, please see https://github.com/aerawave/Awv.Games/blob/master/Awv.Games.WoW/exporting-blizzard-interface-art.md for a guide on how to do this.
        /// </summary>
        static void UpdateArtwork()
        {
            string WoWDirectory = null;
            Alert.Check(WoWDirectory, nameof(WoWDirectory), $"the {nameof(UpdateArtwork)} method", @"This directory will likely be at 'C:\Program Files (x86)\World of Warcraft' on Windows.");
            var blpDirectory = Path.Combine(WoWDirectory, "BlizzardInterfaceArt");
            var pngDirectory = Path.Combine(Directory.GetCurrentDirectory(), "BlizzardInterfaceArt");
            var updater = new ArtworkUpdater("updater.json", blpDirectory, pngDirectory);
            updater.Start();
        }

        /// <summary>
        /// Creates the <see cref="ItemDatabase"/> with exported data from https://wow.tools/.
        /// </summary>
        /// <param name="databaseDirectory">The directory where all your CSV exports are located</param>
        /// <returns></returns>
        static ItemDatabase CreateItemDatabase(string databaseDirectory)
        {
            var database = new ItemDatabase();
            database.ListFile.Load(Path.Combine(databaseDirectory, "listfile.csv"));
            database.ItemClasses.Load(Path.Combine(databaseDirectory, "itemclass.csv"));
            database.ItemSubClasses.Load(Path.Combine(databaseDirectory, "itemSubclass.csv"));
            database.Items.Load(Path.Combine(databaseDirectory, "item.csv"));
            database.ItemSearchNames.Load(Path.Combine(databaseDirectory, "itemsearchname.csv"));
            database.AssociateRecords();
            return database;
        }

        static void ExportWeaponTypesJson(ItemDatabase database, string exportPath)
        {
            var json = database.GenerateWeaponTypesJson();
            var directory = Path.GetDirectoryName(exportPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            File.WriteAllText(exportPath, json.ToString());
        }

        static void ExportIconUsageData(ItemDatabase database, string exportDirectory, ExportFormat format)
        {
            database.ExportIconUsage(exportDirectory, format);
        }

        static void ExportWeaponIconUsageData(ItemDatabase database, string exportDirectory, ExportFormat format)
        {
            var subclasses = database.GetValidWeaponTypes();

            foreach (var subclass in subclasses)
                database.ExportIconUsage(subclass, Path.Combine(exportDirectory, $"{subclass.VerboseName}.{format.GetExtension()}"), format);
        }

        static void ExportItemData()
        {
            // Takes some time....
            string databaseDirectory = null;

            var database = CreateItemDatabase(databaseDirectory);
            var weaponTypesPath = Path.Combine(Directory.GetCurrentDirectory(), "export/weapon-types.json");
            var iconUsageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "export/Icon Usage");
            var format = ExportFormat.PlainText;

            // For whatever reason, "Spears" is a weapon type which has icons associated with it.
            // As a result, this is included in both of the following exports. Can be completely ignored or removed.

            ExportWeaponTypesJson(database, weaponTypesPath);
            ExportWeaponIconUsageData(database, iconUsageDirectory, format);
        }

        static void CustomWoWItems()
        {

        }

    }

    public static class Alert
    {
        public static void Check(string value, string variableName, string location, string resolutionAdvice)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                var alert = new StringBuilder();
                alert.AppendLine($"Have you set the {variableName} variable in {location}?");
                alert.AppendLine("This is required.");
                alert.AppendLine(resolutionAdvice);
                throw new Exception(alert.ToString());
            }
        }
    }
}
