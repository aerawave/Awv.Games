using Awv.Games.Graphics;
using Awv.Games.WoW.Data;
using Awv.Games.WoW.Data.Artwork;
using Awv.Games.WoW.Data.Items;
using Awv.Games.WoW.Graphics;
using Awv.Games.WoW.Items;
using Awv.Games.WoW.Items.Effects;
using Awv.Games.WoW.Items.Equipment;
using Awv.Games.WoW.Levels;
using Awv.Games.WoW.Stats;
using Awv.Games.WoW.Tooltips;
using SixLabors.ImageSharp;
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
            var drawIcon = true;
            var fate = GetDreadbladesFate(drawIcon);
            var provider = new ItemTooltipProvider { DrawIcon = drawIcon };
            var generator = TooltipGenerators.Resolve(fate);

            var tooltip = generator.Generate(provider, fate, 4f);

            using var tooltip1 = File.Open("tooltip-1.png", FileMode.Create);
            tooltip.SaveAsPng(tooltip1);

            // fancy some corruption?
            fate.Stats.Add(new WoWStat(StatType.Corruption, "Corruption", 20));
            fate.EquipEffects.Add(new EquipEffect("Increases the amount of Critical Strike you gain from all sources by 12%.").Corrupt());

            generator = TooltipGenerators.Resolve(fate);

            tooltip = generator.Generate(provider, fate, 4f);

            using var tooltip2 = File.Open("tooltip-2.png", FileMode.Create);
            tooltip.SaveAsPng(tooltip2);

        }

        static Weapon GetDreadbladesFate(bool withIcon)
        {
            // Making this item: https://www.wowhead.com/item=128872
            var item = new Weapon();

            item.Name = "The Dreadblades";
            item.Rarity = ItemRarity.Artifact;
            item.MultiPieceName = "Fate";
            item.ItemLevel = new ItemLevel(152);
            item.BindsOn = "Binds when picked up";
            item.Uniqueness = "Unique";
            item.Type = new EquipmentType.Weapon("Main Hand", "Sword");
            item.MinimumDamage = 41;
            item.MaximumDamage = 69;
            item.AttackSpeed = 2.6M;
            item.Stats.Add(new WoWStat(StatType.Primary, "Agility", 19));
            item.Stats.Add(new WoWStat(StatType.Primary, "Stamina", 28));
            item.Stats.Add(new WoWStat(StatType.Secondary, "Critical Strike", 13));
            item.Stats.Add(new WoWStat(StatType.Secondary, "Mastery", 12));

            if (withIcon)
            {
                string iconDirectory = null;
                Alert.Check(iconDirectory, nameof(iconDirectory), $"the {nameof(GetDreadbladesFate)} function", "This should be the PNG export location with '/Interface/ICONS' appended to the end.");
                var iconGen = new GraphicFileGenerator(iconDirectory);
                item.Icon = iconGen.FindSpecific("inv_sword_1h_artifactskywall_d_01");
            }

            // can't add relic slots or class restrictions

            item.EquipEffects.Add(new EquipEffect("Grants the Curse of the Dreadblades ability, which lets you use finishers more rapidly... at a price."));
            item.Flavor = "She saw herself commanding an impossibly large pirate fleet, one that could conquer the high seas and all the nations of Azeroth. Every ship that dared to challenge her burned, and every city gave up its treasures or was destroyed.";

            item.SellPrice.SetAmount(1898637);

            return item;
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
