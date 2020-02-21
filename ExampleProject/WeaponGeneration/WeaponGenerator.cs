using Awv.Automation.Generation;
using Awv.Automation.Generation.Interface;
using Awv.Automation.Lexica;
using Awv.Automation.Lexica.Compositional;
using Awv.Games.Graphics;
using Awv.Games.WoW.Graphics;
using Awv.Games.WoW.Items;
using Awv.Games.WoW.Items.Effects;
using Awv.Games.WoW.Items.Equipment;
using Awv.Games.WoW.Items.Equipment.Interface;
using Awv.Games.WoW.Levels;
using Awv.Games.WoW.Levels.Interface;
using Awv.Games.WoW.Stats;
using Awv.Lexica.Compositional;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleProject.WeaponGeneration
{
    public class WeaponGenerator : IGenerator<IWeapon>
    {
        public PhraseGenerator Phrases { get; set; }
        public PhraseGenerator SystemPhrases { get; set; }
        public EquipmentIconGenerator Icon { get; set; }
        public RangeGenerator Range { get; set; } = new RangeGenerator();

        public IGenerator<ItemRarity> Rarity { get; set; } = new EnumGenerator<ItemRarity>();
        public StatGenerator PrimaryStats { get; set; }
        public StatGenerator SecondaryStats { get; set; }
        public StatGenerator TertiaryStats { get; set; }
        public StatGenerator CorruptionStats { get; set; }
        public IGenerator<int> PrimaryStatCount { get; set; }
        public IGenerator<int> SecondaryStatCount { get; set; }
        public IGenerator<int> TertiaryStatCount { get; set; }
        public IGenerator<int> CorruptionStatCount { get; set; }
        public IGenerator<EquipmentType> WeaponType { get; set; }
        public IGenerator<string> Name { get; set; }
        public IGenerator<string> Equip { get; set; }
        public IGenerator<string> Use { get; set; }
        public IGenerator<string> Corruption { get; set; }
        public IGenerator<string> Flavor { get; set; }

        public IWeapon Generate(IRNG random)
        {
            var weapon = new Weapon();

            weapon.RequiredLevel = Range.SetRange(1, 120).Generate(random);
            weapon.Rarity = Rarity.Generate(random);
            weapon.ItemLevel = GenerateItemLevel(weapon, random);
            weapon.AttackSpeed = (decimal)Range.SetRange(10, 36).Generate(random) / 10;
            weapon.DefaultDamage = GenerateBaseDamage(weapon, random);
            weapon.Type = WeaponType.Generate(random);

            Icon.EquipmentType = weapon.Type;
            weapon.Icon = Icon.Generate(random);

            weapon.SellPrice.SetAmount(GenerateSellPrice(weapon, random));

            GenerateStats(weapon, random);

            var engine = new CompositionEngine();
            engine.RegisterLibrary<Core>();
            var generatorByWeaponType = Phrases.Tagged(weapon.Type.Definition.Name);

            var nameFormat = new AutomationParser(Name.Generate(random)).Transpile();

            nameFormat.ReplaceTag("weapon", generatorByWeaponType, random, weapon.Type.Definition.Name);

            weapon.Name = nameFormat.Build(engine);
            engine.SetProperty("name", weapon.Name);

            if (engine.GetProperty("shortname") == null)
                engine.SetProperty("shortname", engine.GetProperty("name"));

            var specialItemFlags = SystemPhrases.Tagged("SpecialItemFlag").GenerateManyUnique(random, Math.Max(0, Range.SetRange(-5, 3).Generate(random)));

            foreach (var flag in specialItemFlags)
                weapon.SpecialItemFlags.Add(flag.ToString());

            if (Range.SetRange(0, 100).Generate(random) < 50)
            {
                var useFormat = new AutomationParser(Use.Generate(random)).Transpile();
                useFormat.ReplaceTag("weapon", generatorByWeaponType, random, weapon.Type.Definition.Name);
                weapon.UseEffects.Add(new UseEffect(useFormat.Build(engine)));
            }

            if (Range.SetRange(0, 100).Generate(random) < 50)
            {
                var equipFormat = new AutomationParser(Equip.Generate(random)).Transpile();
                equipFormat.ReplaceTag("weapon", generatorByWeaponType, random, weapon.Type.Definition.Name);
                weapon.EquipEffects.Add(new EquipEffect(equipFormat.Build(engine)));
            }

            if (weapon.IsCorrupted())
            {
                var corruptFormat = new AutomationParser(Corruption.Generate(random)).Transpile();
                corruptFormat.ReplaceTag("weapon", generatorByWeaponType, random, weapon.Type.Definition.Name);
                weapon.EquipEffects.Add(new EquipEffect(corruptFormat.Build(engine)).Corrupt());
            }

            return weapon;
        }

        private void GenerateStats(Weapon weapon, IRNG random)
        {
            GeneratePrimaryStats(weapon, random);
            GenerateSecondaryStats(weapon, random);
            GenerateTertiaryStats(weapon, random);
            GenerateCorruptionStats(weapon, random);
        }

        private void GeneratePrimaryStats(Weapon weapon, IRNG random)
        {
            var count = PrimaryStatCount.Generate(random);
            var points = weapon.ItemLevel.GetTotalStats(weapon.Rarity);
            var deviate = 0;

            for (var i = 0; i < count;i++)
            {
                PrimaryStats.Amount = ((points / count) + deviate);
                var stat = PrimaryStats.Generate(random);
                var amount = stat.GetAmount();
                deviate = Math.Max(Range.SetRange(0, (int)amount * 4).Generate(random) - (int)amount * 2, (int)(points / (count + 1)));

                if (!weapon.HasStat(stat.GetName()) && stat.GetAmount() > 0.5M)
                    weapon.Stats.Add(stat);
            }
        }

        private void GenerateSecondaryStats(Weapon weapon, IRNG random)
        {
            var count = SecondaryStatCount.Generate(random);
            var points = weapon.ItemLevel.GetTotalStats(weapon.Rarity);
            var deviate = 0;

            for (var i = 0; i < count; i++)
            {
                SecondaryStats.Amount = ((points / count) + deviate);
                var stat = SecondaryStats.Generate(random);
                var amount = stat.GetAmount();
                deviate = Math.Max(Range.SetRange(0, (int)amount * 4).Generate(random) - (int)amount * 2, (int)(points / (count + 1)));

                if (!weapon.HasStat(stat.GetName()) && stat.GetAmount() > 0.5M)
                    weapon.Stats.Add(stat);
            }
        }

        private void GenerateTertiaryStats(Weapon weapon, IRNG random)
        {
            var count = TertiaryStatCount.Generate(random);
            var points = weapon.ItemLevel.GetTotalStats(weapon.Rarity);
            var deviate = 0;

            for (var i = 0; i < count; i++)
            {
                TertiaryStats.Amount = ((points / count) + deviate);
                var stat = TertiaryStats.Generate(random);
                var amount = stat.GetAmount();
                deviate = Math.Max(Range.SetRange(0, (int)amount * 4).Generate(random) - (int)amount * 2, (int)(points / (count + 1)));

                if (!weapon.HasStat(stat.GetName()) && stat.GetAmount() > 0.5M)
                    weapon.Stats.Add(stat);
            }
        }

        private void GenerateCorruptionStats(Weapon weapon, IRNG random)
        {
            var count = CorruptionStatCount.Generate(random);
            var points = weapon.ItemLevel.GetTotalStats(weapon.Rarity);

            for (var i = 0; i < count; i++)
            {
                CorruptionStats.Amount = Range.SetRange(5, 20).Generate(random);
                var stat = CorruptionStats.Generate(random);

                if (!weapon.HasStat(stat.GetName()) && stat.GetAmount() > 0.5M)
                    weapon.Stats.Add(stat);
            }
        }

        private long GenerateSellPrice(Weapon weapon, IRNG random)
        {
            var basePrice = (long)Math.Pow(weapon.RequiredLevel, ((double)weapon.RequiredLevel / 150) + 2);

            var maxVariance = GetMaxSellPriceVariance(weapon);
            var change = Range.SetRange(0, maxVariance).Generate(random) - (int)Math.Floor((double)maxVariance / 2);
            return basePrice + (long)change;
        }

        private int GetMaxSellPriceVariance(Weapon weapon)
        {
            var disparity = (int)weapon.Rarity - (int)ItemRarity.Rare;
            var variance = ((int)weapon.Rarity * 7) * (disparity + 4) * 250;

            return (int)Math.Pow(variance, (double)weapon.ItemLevel.GetLevel() / 300);
        }

        private DamageRange GenerateBaseDamage(Weapon weapon, IRNG random)
        {
            var dps = weapon.ItemLevel.GetDPS(weapon.Rarity);
            var speed = weapon.AttackSpeed;
            var damageAverage = (int)(dps * speed);
            var damageTotal = damageAverage * 2;
            var distanceScale = (decimal)Range.SetRange(25, 75).Generate(random) / 100;
            var distance = (int)(distanceScale * damageTotal);
            var halfDistance = (double)distance / 2;
            var max = damageAverage + Math.Ceiling(halfDistance);
            var min = damageAverage - Math.Floor(halfDistance);

            return new DamageRange((decimal)min, (decimal)max);
        }

        private ItemLevel GenerateItemLevel(Weapon weapon, IRNG random)
        {
            var ilvl = weapon.RequiredLevel.GetItemLevel(weapon.Rarity);
            var maxVariance = GetMaxItemLevelVariance(weapon, ilvl);
            var change = Range.SetRange(0, maxVariance).Generate(random) - (int)Math.Floor((double)maxVariance / 2);
            return new ItemLevel(ilvl.GetLevel() + change);
        }

        private int GetMaxItemLevelVariance(Weapon weapon, IItemLevel ilvl)
        {
            var disparity = (int)weapon.Rarity - (int)ItemRarity.Rare;
            var variance = ((int)weapon.Rarity + 7) * (disparity + 4) * 2;
            return (int)(variance * (double)ilvl.GetLevel() / ItemLevel.ArbitraryTurnover);
        }
    }
}
