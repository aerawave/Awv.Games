using Awv.Automation.Generation.Interface;
using Awv.Games.Graphics;
using Awv.Games.WoW.Items.Equipment;
using System.IO;
using System.Linq;

namespace Awv.Games.WoW.Graphics
{
    /// <summary>
    /// Used to generate an icon for a piece of equipment from a given <see cref="IconUsageDirectory"/>. See system-data.md at the root of this project for more details.
    /// </summary>
    public class EquipmentIconGenerator : GraphicFileGenerator
    {
        /// <summary>
        /// Type of equipment to get an icon for.
        /// </summary>
        public EquipmentType EquipmentType { get; set; }
        /// <summary>
        /// The directory which contains icon usage files.
        /// </summary>
        public string IconUsageDirectory { get; set; }

        public EquipmentIconGenerator(string directory, string iconUsageDirectory) : base(directory)
        {
            IconUsageDirectory = iconUsageDirectory;
        }

        public EquipmentIconGenerator(string directory, string iconUsageDirectory, EquipmentType equipmentType) : this(directory, iconUsageDirectory)
        {
            EquipmentType = equipmentType;
        }

        /// <summary>
        /// Generates a graphic with a given <see cref="EquipmentType"/>.
        /// </summary>
        /// <returns>A graphic with a given <see cref="EquipmentType"/></returns>
        public override IGraphic Generate(IRNG random)
        {
            var topdir = EquipmentType.Definition.Type;
            var subdir = EquipmentType.Definition.VerboseName;

            var validResourceNames = File.ReadAllLines(Path.Combine(IconUsageDirectory, topdir, $"{subdir}.txt"));
            var validResourceFiles = validResourceNames.Select(line => Path.Combine(FileGenerator.Resource, $"{line}.png"));
            var previousQuery = FileGenerator.Query;
            FileGenerator.Query = directory => validResourceFiles;

            var graphic = base.Generate(random);

            FileGenerator.Query = previousQuery;

            return graphic;
        }
    }
}
