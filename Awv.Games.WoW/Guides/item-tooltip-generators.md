# Setting up TooltipGenerators
##### *This code can also be found in the ExampleProject.*

This guide relies on the assumption that you have already [exported BlizzardInterfaceArt][1] AND [converted it to PNG format][2].

## Example of a default TooltipGenerator
---
``` c#
// This should be the PNG export location.
string blizzardInterfaceArtDirectory = null;
var generator = new TooltipGenerator();

var borderFolder = Path.Combine(blizzardInterfaceArtDirectory, @"Interface\Tooltips");
// This refers to the assets found in the Awv.Games.WoW project, with the folder name "Assets".
var fillFolder = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Tooltips");
var currencyFolder = Path.Combine(blizzardInterfaceArtDirectory, @"Interface\MONEYFRAME");
var currencyName = "UI-MoneyIcons.png";

var currencyPath = Path.Combine(currencyFolder, currencyName);

generator.Border.Load(Path.Combine(borderFolder, "UI-Tooltip-Border.png"), 16);
generator.Fill.Load(Path.Combine(fillFolder, "Fill-Default.png"), 16);
generator.FillColor = TooltipColors.FillDefault;
generator.Currency.Load(currencyPath, 16);
```


## Example of a corruption TooltipGenerator
---
``` c#
// This should be the PNG export location.
string blizzardInterfaceArtDirectory = null;
var generator = new TooltipGenerator();

var borderFolder = Path.Combine(blizzardInterfaceArtDirectory, @"Interface\Tooltips");
// This refers to the assets found in the Awv.Games.WoW project, with the folder name "Assets".
var fillFolder = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Tooltips");
var currencyFolder = Path.Combine(blizzardInterfaceArtDirectory, @"Interface\MONEYFRAME");
var corruptedItemsFolder = Path.Combine(blizzardInterfaceArtDirectory, @"Interface\CorruptedItems");
var currencyName = "UI-MoneyIcons.png";
var corruptedEmblemName = "CorruptedTooltip.png";

var currencyPath = Path.Combine(currencyFolder, currencyName);

generator.Border.Load(Path.Combine(borderFolder, "UI-Tooltip-Border-Corrupted.png"), 32);
generator.Fill.Load(Path.Combine(fillFolder, "Fill-Corrupt.png"), 32);
generator.FillColor = TooltipColors.FillCorrupted;
generator.Currency.Load(currencyPath, 16);

// This loads in the tooltip's emblem (the little eyeball thing at the top)
var corruptedEmblem = Image.Load<Rgba32>(Path.Combine(corruptedItemsFolder, corruptedEmblemName));
// Crops the emblem to only the part required
corruptedEmblem.Mutate(x => x.Crop(new Rectangle(12, 0, 76, 29)));

// Sets teh emblem, as well as its anchor position.
// For other TooltipGenerators, this can be tweaked to set how far down the rest of the tooltip should be offset.
CorruptedGenerator.Emblem = corruptedEmblem;
CorruptedGenerator.EmblemAnchorY = 10;
```

## Example of a static class to resolve which TooltipGenerator to use
---
``` c#
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
        Alert.Check(blizzardInterfaceArtDirectory, nameof(blizzardInterfaceArtDirectory), $"the static {nameof(TooltipGenerator)} constructor", "This directory will be the root directory for where your exported PNG files are that were converted from BLP files.");

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
```

[1]:export-artwork.md
[2]:convert-artwork-to-png.md