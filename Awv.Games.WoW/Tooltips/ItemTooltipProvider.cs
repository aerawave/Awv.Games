using Awv.Games.WoW.Items;
using Awv.Games.WoW.Items.Equipment.Interface;
using Awv.Games.WoW.Stats;
using Awv.Games.WoW.Tooltips.Interface;
using Awv.Games.WoW.Tooltips.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Games.WoW.Tooltips
{
    public class ItemTooltipProvider : ITooltipProvider<IItem>
    {
        public bool DrawIcon { get; set; } = true;

        public bool ShouldDrawIcon(IItem item) => DrawIcon;
        public IEnumerable<ITooltipSection> GetSections(IItem item)
        {
            var sections = new List<ITooltipSection>();

            sections.Add(GetHeader(item));
            sections.Add(GetCore(item));
            sections.Add(GetStats(item));
            sections.Add(GetEffects(item));
            sections.Add(GetFooter(item));

            return sections;
        }

        public TooltipText GetTitle(IItem item) => new TooltipText(item.GetName(), TooltipColors.ToColor(item.GetRarity()));

        public ITooltipSection GetHeader(IItem item)
        {
            var equipment = item as IEquipment;
            var section = new TooltipSection();

            if (item is IEquipment && equipment.IsMultiEquipment())
            {
                var title = GetTitle(item);
                section.Lines.Add(new LeftText(new TooltipText(equipment.GetMultiPieceName(), title.Color)));
            }

            var flags = item.GetSpecialItemFlags();
            if (flags.Count() > 0)
                section.Lines.Add(new LeftText(new TooltipText(string.Join(" ", flags), TooltipColors.Lime)));

            var usage = item.GetUsage();
            if (usage != null)
                section.Lines.Add(new LeftText(new TooltipText(usage, TooltipColors.Yellow)));

            section.Lines.Add(new LeftText(new TooltipText($"Item Level {item.GetItemLevel().GetLevel()}", TooltipColors.Yellow)));

            var bindsOn = item.GetBindsOn();
            if (!string.IsNullOrWhiteSpace(bindsOn))
                section.Lines.Add(new LeftText(bindsOn));

            var uniqueness = item.GetUniqueness();
            if (!string.IsNullOrWhiteSpace(uniqueness))
                section.Lines.Add(new LeftText(uniqueness));

            var type = item.GetItemType();
            if (!string.IsNullOrWhiteSpace(type))
                section.Lines.Add(new LeftText(new TooltipText(type, TooltipColors.ItemType)));

            return section;
        }

        public ITooltipSection GetCore(IItem item)
        {
            var section = new TooltipSection();
            if (item is IEquipment)
            {
                var equipment = item as IEquipment;

                var type = equipment.GetEquipmentType();
                if (type != null)
                {
                    var typeLine = new TooltipLine();
                    typeLine.LeftText = new LeftText(type.Slot).Text;
                    if (type.Definition?.Name != null)
                        typeLine.RightText = new RightText(type.Definition.Name).Text;
                    section.Lines.Add(typeLine);
                }
            }
            if (item is IWeapon)
            {
                var weapon = item as IWeapon;
                var damages = weapon.GetDamageRanges();
                var firstAttack = damages.First();
                var extra = damages.Skip(1);
                var attackSpeed = weapon.GetAttackSpeed();

                var attackLine = new LeftText(firstAttack.GetDisplayString())
                    + new RightText($"Speed {attackSpeed.ToString("N2")}");

                section.Lines.Add(attackLine);

                foreach (var damage in extra)
                    section.Lines.Add(new LeftText($"+ {damage.GetDisplayString()}"));
                var damagePerSecond = attackSpeed > 0 ? damages.Sum(damage => damage.GetMinimum() + damage.GetMaximum()) / (2 * attackSpeed) : 0;

                section.Lines.Add(new LeftText($"({damagePerSecond.ToString("N1")} damage per second)"));

                return section;
            }
            if (item is IArmor)
            {
                var armor = item as IArmor;
                section.Lines.Add(new LeftText($"{armor.GetArmorPoints()} Armor"));
            }
            return section;
        }

        public ITooltipSection GetStats(IItem item)
        {
            var section = new TooltipSection();

            if (item is IEquipment)
            {
                var equipment = item as IEquipment;
                var stats = equipment.GetStats();

                var primaryStats = stats.Where(stat => stat.GetStatType() == StatType.Primary).ToList();
                var secondaryStats = stats.Where(stat => stat.GetStatType() == StatType.Secondary).ToList();
                var tertiaryStats = stats.Where(stat => stat.GetStatType() == StatType.Tertiary).ToList();
                var corruptionStats = stats.Where(stat => stat.GetStatType() == StatType.Corruption).ToList();

                primaryStats.ForEach(stat => section.Lines.Add(new LeftText(new TooltipText(stat.GetDisplayString(), TooltipColors.Common))));
                secondaryStats.ForEach(stat => section.Lines.Add(new LeftText(new TooltipText(stat.GetDisplayString(), TooltipColors.Uncommon))));
                tertiaryStats.ForEach(stat => section.Lines.Add(new LeftText(new TooltipText(stat.GetDisplayString(), TooltipColors.Uncommon))));
                corruptionStats.ForEach(stat => section.Lines.Add(new LeftText(new TooltipText(stat.GetDisplayString(), TooltipColors.CorruptEffect))));

                // TODO (?):
                // artifact relic slots
                // gem sockets
                // socket bonus


                var durability = equipment.GetDuration();
                if (equipment.HasDurability())
                    section.Lines.Add(new LeftText($"Durability {durability} / {durability}"));

            }

            // TODO (?):
            // class restrictions

            return section;
        }

        public ITooltipSection GetEffects(IItem item)
        {
            var section = new TooltipSection();

            var effects = item.GetEffects();

            foreach (var effect in effects)
                section.Lines.Add(new ParagraphLine(new TooltipText($"{effect.GetOrigin()}: {effect.GetEffect()}", effect.GetColor())));

            return section;
        }

        public ITooltipSection GetFooter(IItem item)
        {
            var section = new TooltipSection();


            var level = item.GetRequiredLevel()?.GetLevel();
            if (level.HasValue && level.Value > 0)
                section.Lines.Add(new LeftText($"Requires Level {level.Value}"));

            if (this is IEquipment)
            {
                // TODO (?):
                // set name
                // set pieces
                // set effects

                // also Azerite Gear*
            }

            var duration = item.GetDuration();
            if (duration.HasValue)
                section.Lines.Add(new LeftText($"Duration: {duration.Value.GetTooltipDisplayString()}"));

            var stack = item.GetMaxStack();
            if (stack > 0)
                section.Lines.Add(new LeftText($"Max Stack: {stack}"));

            var flavor = item.GetFlavor();
            if (!string.IsNullOrWhiteSpace(flavor))
                section.Lines.Add(new ParagraphLine(new TooltipText($"\"{flavor}\"", TooltipColors.Flavor)));

            var sellPrice = item.GetSellPrice();
            if (sellPrice != null)
            {
                var gold = (int?)sellPrice.GetAmount("g") ?? 0;
                var silver = (int?)sellPrice.GetAmount("s") ?? 0;
                var copper = (int?)sellPrice.GetAmount("c") ?? 0;
                section.Lines.Add(new CurrencyLine(gold, silver, copper));
            }

            return section;
        }

        public Image<Rgba32> GetIcon(IItem target) => target.GetIcon().GetImage();
    }
}
