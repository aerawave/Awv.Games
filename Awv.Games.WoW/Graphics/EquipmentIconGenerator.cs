using Awv.Automation.Generation.Interface;
using Awv.Games.Graphics;
using Awv.Games.WoW.Items.Equipment;
using System.IO;
using System.Linq;

namespace Awv.Games.WoW.Graphics
{
    public class EquipmentIconGenerator : GraphicFileGenerator
    {
        public EquipmentType EquipmentType { get; set; }
        public string IconUsageDirectory { get; set; }

        public EquipmentIconGenerator(string directory, string iconUsageDirectory) : base(directory)
        {
            IconUsageDirectory = iconUsageDirectory;
        }

        public EquipmentIconGenerator(string directory, string iconUsageDirectory, EquipmentType equipmentType) : this(directory, iconUsageDirectory)
        {
            EquipmentType = equipmentType;
        }

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
