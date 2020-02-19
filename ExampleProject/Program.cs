using Awv.Games.WoW.Data;
using Awv.Games.WoW.Data.Artwork;
using Awv.Games.WoW.Data.Items;
using System;
using System.IO;

namespace ExampleProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ExportItemData();
        }

        /// <summary>
        /// An example of how to use the <see cref="ArtworkUpdater"/>.
        /// 
        /// This will run through all files in a given directory (the "blpDirectory" variable below) and convert every *.blp file to a *.png file, and save it to the output directory (the "pngDirectory" variable below).
        /// </summary>
        static void UpdateArtwork()
        {
            string WoWDirectory = null;
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
            var database = CreateItemDatabase(@"\\awvserv\Apps\Data\Exported (wow.tools)");
            var weaponTypesPath = Path.Combine(Directory.GetCurrentDirectory(), "export/weapon-types.json");
            var iconUsageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "export/Icon Usage");
            var format = ExportFormat.PlainText;

            // For whatever reason, "Spears" is a weapon type which has icons associated with it.
            // As a result, this is included in both of the following exports. Can be completely ignored or removed.

            ExportWeaponTypesJson(database, weaponTypesPath);
            ExportWeaponIconUsageData(database, iconUsageDirectory, format);
        }

    }
}
