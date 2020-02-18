using Awv.Games.WoW.Data.Data.Items.DataTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Awv.Games.WoW.Data.Data.Items
{
    public class ItemDatabase
    {
        /// <summary>
        /// See https://wow.tools/files
        /// </summary>
        public ListFile ListFile { get; set; } = new ListFile();
        /// <summary>
        /// See https://wow.tools/dbc/?dbc=item
        /// </summary>
        public DataTable<Item> Items { get; set; } = new DataTable<Item>();
        /// <summary>
        /// See https://wow.tools/dbc/?dbc=itemclass
        /// </summary>
        public DataTable<ItemClass> ItemClasses { get; set; } = new DataTable<ItemClass>();
        /// <summary>
        /// See https://wow.tools/dbc/?dbc=itemsubclass
        /// </summary>
        public DataTable<ItemSubClass> ItemSubClasses { get; set; } = new DataTable<ItemSubClass>();
        /// <summary>
        /// See https://wow.tools/dbc/?dbc=itemsearchname
        /// </summary>
        public DataTable<ItemSearchName> ItemSearchNames { get; set; } = new DataTable<ItemSearchName>();

        public void AssociateRecords()
        {
            Parallel.ForEach(ItemSubClasses.Values, isc =>
            {
                isc.Class = ItemClasses.Values.FirstOrDefault(ic => ic.ClassId == isc.ClassId);
            });

            Parallel.ForEach(Items.Values, item =>
            {
                item.Class = ItemClasses.Values.FirstOrDefault(ic => ic.ClassId == item.ClassId);
                item.SubClass = ItemSubClasses.Values.FirstOrDefault(isc => isc.Class == item.Class && isc.SubClassId == item.SubClassId);
            });

            Parallel.ForEach(ItemSearchNames.Values, sn =>
            {
                var item = Items.Values.FirstOrDefault(it => it.Id == sn.Id);
                if (item == null) return;
                item.Name = sn.Name;
                item.Rarity = sn.Rarity;
                item.ExpansionId = sn.ExpansionId;
            });

            if (ListFile != null && ListFile.Count > 0)
            {
                Parallel.ForEach(Items.Values, item =>
                {
                    if (ListFile.ContainsKey(item.IconFileId))
                    {
                        item.IconFilePath = ListFile[item.IconFileId];
                        item.IconFileName = Path.GetFileNameWithoutExtension(item.IconFilePath);
                    }
                });
            }
        }

        public void GenerateIconUsage(ItemSubClass subclass, Stream stream, ExportFormat format = ExportFormat.PlainText)
        {
            var items = Items.Values.Where(item => item.SubClass == subclass && !string.IsNullOrWhiteSpace(item.IconFileName)).ToArray();
            using var writer = new StreamWriter(stream);

            var values = items.Select(item => item.IconFileName).ToArray();

            switch (format)
            {
                case ExportFormat.PlainText:
                    foreach (var value in values)
                        writer.WriteLine(value);
                    break;
                case ExportFormat.Json:
                    writer.WriteLine(new JArray(values).ToString());
                    break;
                default:
                    throw new ArgumentException($"{format} is not a valid generate format.", nameof(format));
            }
        }

        public void GenerateIconUsage(ItemSubClass subclass, string filepath, ExportFormat format = ExportFormat.PlainText)
        {
            var directory = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            GenerateIconUsage(subclass, File.Open(filepath, FileMode.Create), format);
        }

        public void GenerateIconUsage(ItemClass itemClass, string directory, ExportFormat format = ExportFormat.PlainText)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var subclasses = ItemSubClasses.Values.Where(isc => isc.Class == itemClass).ToArray();

            foreach (var subclass in subclasses)
                GenerateIconUsage(subclass, Path.Combine(directory, $"{subclass.VerboseName ?? subclass.Name}.{format.GetExtension()}"), format);
        }

        public void GenerateIconUsage(string directory, ExportFormat format = ExportFormat.PlainText)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var itemClasses = ItemClasses.Values.ToArray();

            foreach (var itemClass in itemClasses)
                GenerateIconUsage(itemClass, Path.Combine(directory, itemClass.Name), format);
        }
        public ItemSubClass[] GetValidWeaponTypes()
        {
            var itemClass = ItemClasses.Values.FirstOrDefault(ic => ic.Name == "Weapon");
            var weaponClasses = ItemSubClasses.Values
                .Where(isc => isc.Class == itemClass)
                .Where(isc => !string.IsNullOrWhiteSpace(isc.VerboseName))
                .Where(isc => Items.Values.Count(item => item.SubClass == isc && item.IconFileName != null) > 0)
                .ToArray();
            return weaponClasses;
        }

        public JArray GenerateWeaponTypesJson()
        {
            var weaponClasses = GetValidWeaponTypes();
            var weaponTypes = new JArray();

            var slotContainers = new Dictionary<string, string[]>();


            var oneHandSlots = new string[] { "One-Handed", "Main Hand", "Off-Hand" };
            var twoHandSlots = new string[] { "Two-Handed" };
            var rangedSlots = new string[] { "Ranged" };

            slotContainers.Add("One-Handed", oneHandSlots);
            slotContainers.Add("Warglaives", oneHandSlots);
            slotContainers.Add("Daggers", oneHandSlots);
            slotContainers.Add("Fist Weapons", oneHandSlots);

            slotContainers.Add("Two-Handed", twoHandSlots);
            slotContainers.Add("Polearms", twoHandSlots);
            slotContainers.Add("Staves", twoHandSlots);
            slotContainers.Add("Fishing Poles", twoHandSlots);

            slotContainers.Add("Bows", rangedSlots);
            slotContainers.Add("Guns", rangedSlots);
            slotContainers.Add("Crossbows", rangedSlots);
            slotContainers.Add("Thrown", rangedSlots);
            slotContainers.Add("Wands", rangedSlots);

            foreach (var weaponClass in weaponClasses)
            {
                var weaponType = new JObject();
                var slots = new JArray();

                foreach (var slotKey in slotContainers.Keys)
                    if (weaponClass.VerboseName.Contains(slotKey))
                        foreach (var slot in slotContainers[slotKey])
                            slots.Add(slot);

                weaponType["verboseName"] = weaponClass.VerboseName;
                weaponType["name"] = weaponClass.Name;
                weaponType["type"] = weaponClass.Class.Name;
                weaponType["slots"] = slots;
                weaponTypes.Add(weaponType);
            }

            return weaponTypes;
        }

        public override string ToString() => $"Count = {Items.Count}";
    }

}
