using Awv.Games.WoW.Graphics;
using Awv.Games.WoW.Items;
using Awv.Games.WoW.Tooltips;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System.IO;

namespace ExampleProject
{
    public static class TooltipGenerators
    {
        public static readonly TooltipGenerator DefaultGenerator = new TooltipGenerator();
        public static readonly TooltipGenerator CorruptedGenerator = new TooltipGenerator();

        public static TooltipGenerator Resolve(IItem item)
        {
            if (item.IsCorrupted()) return CorruptedGenerator;
            else return DefaultGenerator;
        }

        static TooltipGenerators()
        {
            string blizzardInterfaceArtDirectory = null;

            var fillFolder = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Tooltips");
            var borderFolder = Path.Combine(blizzardInterfaceArtDirectory, @"Interface\Tooltips");
            var corruptedItemsFolder = Path.Combine(blizzardInterfaceArtDirectory, @"Interface\CorruptedItems");
            var currencyFolder = Path.Combine(blizzardInterfaceArtDirectory, @"Interface\MONEYFRAME");
            var currencyName = "UI-MoneyIcons.png";
            var corruptedEmblemName = "CorruptedTooltip.png";

            var currencyPath = Path.Combine(currencyFolder, currencyName);

            DefaultGenerator.Border.Load(Path.Combine(borderFolder, "UI-Tooltip-Border.png"), 16);
            DefaultGenerator.Fill.Load(Path.Combine(fillFolder, "Fill-Default.png"), 16);
            DefaultGenerator.FillColor = TooltipColors.FillDefault;
            DefaultGenerator.Currency.Load(currencyPath, 16);

            CorruptedGenerator.Border.Load(Path.Combine(borderFolder, "UI-Tooltip-Border-Corrupted.png"), 32);
            CorruptedGenerator.Fill.Load(Path.Combine(fillFolder, "Fill-Corrupt.png"), 32);
            CorruptedGenerator.FillColor = TooltipColors.FillCorrupted;
            CorruptedGenerator.Currency.Load(currencyPath, 16);

            var corruptedEmblem = Image.Load<Rgba32>(Path.Combine(corruptedItemsFolder, corruptedEmblemName));
            corruptedEmblem.Mutate(x => x.Crop(new Rectangle(12, 0, 76, 29)));

            CorruptedGenerator.Emblem = corruptedEmblem;
            CorruptedGenerator.EmblemAnchorY = 10;
        }
    }
}
