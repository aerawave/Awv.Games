# Automated Generation

##### *This code can be found in the ExampleProject. Any code referenced will not be duplicated here, as there's too much.*

This guide relies on the assumption that you have completed the first guides in the [How To Use Awv.Games.WoW][1] documentation.

This is a bit of a heavier guide than the others, as a lot more goes into it.

That said, this is much easier to understand if you look at the project code along side the documentation.

## Base Generators
---
First, we're going to need a few generators to get us started. These will be the following:
- [CountGenerator.cs] for generating the counts of stats we'll apply to weapons
- [StatGenerator.cs] for, well, generating the stats.
- [WeaponTypeGenerator.cs] for generating an `EquipmentType` for our weapon.

### CountGenerator
This sortof acts as a weighted version of the stock `RangeGenerator`. Each increment, it checks the current count against a property called the `ChanceFunc` to see if it should continue incrementing. The reason for this is so that

### StatGenerator
This generates `IWoWStat`s for us.

### WeaponTypeGenerator
This will generate `EquipmentType`s for us. It specifically filters `EquipmentTypeDefinition`s which are of type "Weapon", and creates an instance of `EquipmentType` for each slot on that definition.

## Core Library
---
Feel free to use whatever library you want, including making your own, but I've provided [Core.cs] to get you started. This is to serve as an instance of `ILibrary` for your `CompositionEngine` to use. (more on that below)

## Weapon Generator
---
Once you have your various generators in place, you'll need a Weapon generator. I have provided one for this example, which can be found here: [WeaponTypeGenerator.cs]

The code for that generator uses other generators to compile together different information for weapon generation. After gathering all the non text-based information, it then starts to generate text-based information using a running `CompositionEngine`. See below for a small snippet of this in action:

``` c#
if (Range.SetRange(0, 100).Generate(random) < 50)
{
    var useFormat = new AutomationParser(Use.Generate(random)).Transpile();
    useFormat.ReplaceTag("weapon", generatorByWeaponType, random, weapon.Type.Definition.Name);
    weapon.UseEffects.Add(useFormat.Build(engine));
}
```

The above code generates a number between 0 and 100, then if it is below 50 (representing a 50% chance), a Use format is generated, taken into an `AutomationParser` which is a tool for fulfilling data and information.

Once that format is generated (in the form of the `useFormat` variable), the format is told to replace any instances of `TagLexigram`s which reference the tag "weapon" with a phrase which corresponds to the type of weapon being produced.

This is kinda worded pretty badly, but simply put, this makes it so that when you have an equip effect on, for instance, a Two Handed Axe, the equip effect reads `This #weapon increases your Critical Strike by 50.` or something to that effect, it doesn't fulfill that `#weapon` tag with "Staff" or some other irrelevant weapon type.

Custom formatting can be done wherever needed similar to this.

It's worth noting that the `Composition.ReplaceTag` method is an extension method provided in the example project. It is not a part of base Compositions. That can be found here: [CompositionExtensions.cs]

## GenerateWeaponsProgram
---
This all comes together in the [GenerateWeaponsProgram.cs] class which is just a static class which brings together the various generators and invokes them.

[1]:how-to-use.md
[Core.cs]:../../ExampleProject/WeaponGeneration/Core.cs
[WeaponGenerator.cs]:../../ExampleProject/WeaponGeneration/WeaponGenerator.cs
[GenerateWeaponsProgram.cs]:../../ExampleProject/GenerateWeaponprogram.cs
[CountGenerator.cs]:../../ExampleProject/WeaponGeneration/CountGenerator.cs
[StatGenerator.cs]:../../ExampleProject/WeaponGeneration/StatGenerator.cs
[WeaponTypeGenerator.cs]:../../ExampleProject/WeaponGeneration/WeaponTypeGenerator.cs
[CompositionExtensions.cs]:../../ExampleProject/CompositionExtensions.cs