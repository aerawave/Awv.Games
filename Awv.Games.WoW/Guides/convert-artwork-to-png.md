# Converting BlizzardInterfaceArt to PNG format

Feel completely free to make your own process for doing this, but in the event you'd like to just use a pre-built solution, check out Awv.Games.WoW.Data.Artwork.ArtworkUpdater!

## Using your own custom solution

A tool I'd recommend using for this is called [libwarcraft][1] by [Nihlus][2] on Github.

This is one of the two tools that were pivotal to making the WoW Weapons Bot possible.

## Using the Using Awv.Games.WoW.Data pre-built solution

##### *This code can also be found in the ExampleProject.*
``` c#
// Set blpDirectory to your BlizzardInterfaceArt directory
string blpDirectory = null;
// Set pngDirectory to where you want to output PNG files.
var pngDirectory = Path.Combine(Directory.GetCurrentDirectory(), "BlizzardInterfaceArt");
// Create an ArtworkUpdater.
// The "updater.json" is a config file used to track where the updater is.
// Used in the event that you need to pause the operation and resume it later.
var updater = new ArtworkUpdater("updater.json", blpDirectory, pngDirectory);
// Start it! It will log process as it goes, as well as report any errors to the aforementioned JSON file.
updater.Start();
```

[1]:https://www.nuget.org/packages/libwarcraft
[2]:https://github.com/Nihlus