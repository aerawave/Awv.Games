using Awv.Automation.Generation;
using Awv.Automation.Lexica;
using Awv.Games.WoW.Graphics;
using Awv.Games.WoW.Items.Equipment;
using Awv.Games.WoW.Stats;
using Awv.Games.WoW.Tooltips;
using ExampleProject.WeaponGeneration;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExampleProject
{
    public static class GenerateWeaponsProgram
    {
        public static void Run()
        {
            var phrases = GetPhrases();
            var weaponTypes = GetWeaponTypes(Paths.WeaponTypes);
            Core.RNG = new RNG();
            Core.Generator = new WeaponGenerator();
            Core.Generator.Phrases = new PhraseGenerator(phrases);
            Core.Generator.SystemPhrases = new PhraseGenerator(phrases);
            Core.Generator.PrimaryStats = new StatGenerator(StatType.Primary, phrases.Where(phrase => phrase.IsTagged("primarystat")));
            Core.Generator.PrimaryStatCount = new CountGenerator(2, 3, 0.5d);
            Core.Generator.SecondaryStats = new StatGenerator(StatType.Secondary, phrases.Where(phrase => phrase.IsTagged("secondarystat")));
            Core.Generator.SecondaryStatCount = new CountGenerator(0, 3, 0.5d);
            Core.Generator.TertiaryStats = new StatGenerator(StatType.Tertiary, phrases.Where(phrase => phrase.IsTagged("secondarystat")));
            Core.Generator.TertiaryStatCount = new CountGenerator(0, 2, 0.2d);
            Core.Generator.CorruptionStats = new StatGenerator(StatType.Corruption, phrases.Where(phrase => phrase.IsTagged("corruptionstat")));
            Core.Generator.CorruptionStatCount = new CountGenerator(0, 1, 0.05d);
            Core.Generator.Icon = new EquipmentIconGenerator(Paths.Icons, Paths.IconUsage);
            Core.Generator.WeaponType = new WeaponTypeGenerator(weaponTypes);
            Core.Generator.Name = new PredefinedGenerator<string>(GetNameFormats());
            Core.Generator.Equip = new PredefinedGenerator<string>(GetEquipFormats());
            Core.Generator.Use = new PredefinedGenerator<string>(GetUseFormats());
            Core.Generator.Corruption = new PredefinedGenerator<string>(GetCorruptionFormats());
            Core.Generator.Flavor = new PredefinedGenerator<string>(GetFlavors());

            var weapon = Core.Generator.Generate(Core.RNG);

            var ttgen = TooltipGenerators.Resolve(weapon);
            var provider = new ItemTooltipProvider();

            var tooltip = ttgen.Generate(provider, weapon, 4f);

            var guid = Guid.NewGuid();
            using var fs = File.Open($"gen/{guid}.png", FileMode.Create);

            tooltip.SaveAsPng(fs);
        }

        static EquipmentTypeDefinition[] GetWeaponTypes(string weaponTypesJsonPath)
        {
            var fileContents = File.ReadAllText(weaponTypesJsonPath);
            var jarray = JArray.Parse(fileContents);
            var definitions = new List<EquipmentTypeDefinition>();
            foreach (var token in jarray)
                definitions.Add(token.ToObject<EquipmentTypeDefinition>());
            return definitions.ToArray();
        }

        static string[] GetNameFormats()
        {
            var list = new List<string>();

            list.Add("{#element()#rage:L}(shortname), #worked() #weaponhead() of the #element(element2)seeker");
            list.Add("{#status }`.5`{#class's }`.5`#weapon");
            list.Add("#locationed {#rank }`.5`#weapon");

            return list.ToArray();
        }

        static string[] GetEquipFormats()
        {
            var list = new List<string>();

            list.Add("Gain `secondarystat()` #secondarystat.");
            list.Add("#skill skill increased by `ri(1,99)`.");
            list.Add("Your attacks have a chance to trigger an extra attack{ `ri(1,15)` times}`0.05`.");
            list.Add("{This weapon}(shortname) emanates raw power.");

            return list.ToArray();
        }

        static string[] GetUseFormats()
        {
            var list = new List<string>();

            list.Add("Set the total pet maximum for your account to `ri(500,3000)`. Usable once per account. Consumed on use.");
            list.Add("Creates a portal, teleporting group members that use it to #location. (`ri(1,59)` Min Cooldown)");
            list.Add("Request a pickup to the nearest flight master.\n\nOnly works in #location.");

            return list.ToArray();
        }

        static string[] GetCorruptionFormats()
        {
            var list = new List<string>();

            list.Add("Increases the amount of #secondarystat you gain from all sources by `ri(1,3)*4`%.");
            list.Add("Your spells and abilities have a chance to increase your #secondarystat by `ri(1,4)*seconraystat()` for 4 sec.");
            list.Add("Your damaging abilities build the Echoing Void. Each time it builds, Echoing Void has a chance to collapse, dealing `ri(1,2)*.8+ri(0,1)*.4`% of your Health as #element damage to all nearby enemies every 1 sec until no stacks remain.");

            return list.ToArray();
        }

        static string[] GetFlavors()
        {
            var list = new List<string>();

            list.Add("Disclaimer: Due to the functionality of this product, slight #ailments, #ailment and race and/or gender changes have been known to occur. Use with caution.");
            list.Add("Safety {not }`.5`guaranteed.");
            list.Add("The initials #letter.#letter. are etched on the #weaponpart.");

            return list.ToArray();
        }

        static Phrase[] GetPhrases()
        {
            var phrases = new List<Phrase[]>();

            phrases.Add(GetPhrases("element",
            @"Lightning
            Water
            Shadow
            Spirit
            Mist
            Energy
            Power
            Light
            Darkness
            Life
            Flame
            Shock
            Thunder
            Arcane
            Fire
            Fel
            Ice
            Wind
            Venom
            Nature"));

            phrases.Add(GetPhrases("rage",
            @"Fury
            Rage
            Hatred"));

            phrases.Add(GetPhrases("worked",
            @"Enchanted
            Steelforged
            Infused
            Engraved
            Hollowed
            Carved
            Stained
            Hardened
            Wrapped
            Vinewrapped
            Tipped
            Warforged
            Titanforged
            Forged
            Etched
            Lightforged
            Blessed"));

            phrases.Add(GetPhrases("weaponhead",
            @"Razor
            Blade
            Blades
            Barb
            Barbs
            Spike
            Spikes
            Hook
            Point"));

            phrases.Add(GetPhrases("status", "Notorious"));

            phrases.Add(GetPhrases("class",
            @"Tidemaiden
            Deepwarden
            Overseer
            Butcher
            War-Caller
            Footman
            Wavecaller
            Trapper
            Hunter
            Gladiator
            Captain
            Tempest
            Myrmidon
            Blade Dancer
            High Sage
            Sage
            Druid
            Mage
            Paladin
            Priest
            Rogue
            Shaman
            Warlock
            Warrior
            Death Knight
            Monk
            Demon Hunter
            Stomreaver
            Engineer
            Exarch
            Keeper"));

            phrases.Add(GetPhrases("weapon",
            @"Spellhammer
            Smasher
            Scepter
            Gavel
            Hammer
            Warmace
            Warhammer
            Claw
            Claws
            Javelin
            Cestus
            Fist
            Fists
            Stinger
            Spikefist
            Knuckle
            Knuckles
            Armblade
            Chainfist
            Talon
            Talons
            Hawktalons
            Gutripper
            Razor
            Blade
            Blades
            Sunclaw
            Barb
            Barbs
            Spike
            Spikes
            Fang
            Hand
            Bite
            Lancet
            Shank
            Hook
            Tusk
            Tusks
            Mindscythe
            Jaw
            Jaws
            Maw
            Scythe
            Star
            Bladed Fan
            Katar
            Geoknife
            Handblade
            Slicer
            Bladefist
            Perforator
            Icefist
            Warblades
            Hand Blade"));

            phrases.Add(GetPhrases("primarystat",
            @"Intellect
            Agility
            Strength
            Stamina"));

            phrases.Add(GetPhrases("secondarystat",
            @"Critical Strike
            Haste
            Mastery
            Versatility"));

            phrases.Add(GetPhrases("tertiarystat",
            @"Speed
            Indestructible
            Leech
            Avoidance"));

            phrases.Add(GetPhrases("skill",
            @"Fishing
            Herbalism"));

            phrases.Add(GetPhrases("locationed",
            @"Gorian
            Rangari
            Borean
            Barbthorn
            Bonelash
            Mardenholde
            Steelspark
            Razorwind
            Splinterspear
            Ironspine
            Spleenripper
            The Sunwell
            Scarlet
            The Cosmos
            Highland
            Northrend
            Winter's Breath Lake
            Shadowmoon Valley"));

            phrases.Add(GetPhrases("rank",
            @"King
            Queen
            Initiate
            Gladiator
            Captain
            Tempest
            Myrmidon
            Combatant
            High Sage
            Wakener
            Marshal
            Grand Marshal
            Highlord
            Paragon
            Exarch
            Maiden"));

            phrases.Add(GetPhrases("corruptionstat", "Corruption"));

            phrases.Add(GetPhrases("letter",
            @"L
            A
            B
            C
            D
            E
            F
            G
            H
            I
            J
            K
            M
            N
            O
            P
            Q
            R
            S
            T
            U
            V
            W
            X
            Y
            Z"));

            phrases.Add(GetPhrases("weaponpart",
            @"Razor
            Blade
            Blades
            Barb
            Barbs
            Spike
            Spikes
            Hook
            Point
            Hilt"));

            phrases.Add(GetPhrases("ailment",
            @"Headache
            Nausea"));

            phrases.Add(GetPhrases("specialitemflag",
            @"Heroic
            Mythic
            Warforged
            Titanforged"));

            phrases.Add(GetPhrases("location",
            @"Karazhan
            Hidden Pass
            The Abyss
            Salt Coast
            Warport
            Rivermarsh
            Mardenholde
            Nexus
            The Sunwell
            Pandaria
            The Cosmos
            Argus
            Ny'alotha, the Waking City
            Ny'alotha
            Northrend
            Winter's Breath Lake
            Shadowmoon Valley
            The Ghost World"));

            phrases.Add(GetPhrases("ailments", "Headaches"));

            return phrases.SelectMany(ph => ph).ToArray();
        }

        static Phrase[] GetPhrases(string tag, string phrases)
        {
            return phrases.Split('\n').Select(str => str.Trim()).Select(str => new Phrase(str, tag)).ToArray();
        }
    }
}
