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
        public IEnumerable<ITooltipSection> GetSegments(IItem item)
        {
            var segments = new List<ITooltipSection>();

            segments.Add(GetHeaderSegment(item));
            segments.Add(GetCoreSegment(item));
            segments.Add(GetStatsSegment(item));
            segments.Add(GetEffectsSegment(item));
            segments.Add(GetFooterSegment(item));

            return segments;
        }

        public TooltipText GetTitle(IItem item) => new TooltipText(item.GetName(), TooltipColors.ToColor(item.GetRarity()));

        public ITooltipSection GetHeaderSegment(IItem item)
        {
            var equipment = this as IEquipment;
            var segment = new TooltipSection();

            if (this is IEquipment && equipment.IsMultiEquipment())
            {
                var title = GetTitle(item);
                segment.Lines.Add(new LeftText(new TooltipText(equipment.GetMultiPieceName(), title.Color)));
            }

            var flags = item.GetSpecialItemFlags();
            if (flags.Count() > 0)
                segment.Lines.Add(new LeftText(new TooltipText(string.Join(" ", flags), TooltipColors.Lime)));

            var usage = item.GetUsage();
            if (usage != null)
                segment.Lines.Add(new LeftText(new TooltipText(usage, TooltipColors.Yellow)));

            segment.Lines.Add(new LeftText(new TooltipText($"Item Level {item.GetItemLevel().GetLevel()}", TooltipColors.Yellow)));

            var bindsOn = item.GetBindsOn();
            if (!string.IsNullOrWhiteSpace(bindsOn))
                segment.Lines.Add(new LeftText(bindsOn));

            var uniqueness = item.GetUniqueness();
            if (!string.IsNullOrWhiteSpace(uniqueness))
                segment.Lines.Add(new LeftText(uniqueness));

            var type = item.GetItemType();
            if (!string.IsNullOrWhiteSpace(type))
                segment.Lines.Add(new LeftText(new TooltipText(type, TooltipColors.ItemType)));

            return segment;
        }

        public ITooltipSection GetCoreSegment(IItem item)
        {
            var segment = new TooltipSection();
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
                    segment.Lines.Add(typeLine);
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

                segment.Lines.Add(attackLine);

                foreach (var damage in extra)
                    segment.Lines.Add(new LeftText($"+ {damage.GetDisplayString()}"));
                var damagePerSecond = attackSpeed > 0 ? damages.Sum(damage => damage.GetMinimum() + damage.GetMaximum()) / (2 * attackSpeed) : 0;

                segment.Lines.Add(new LeftText($"({damagePerSecond.ToString("N1")} damage per second)"));

                return segment;
            }
            if (item is IArmor)
            {
                var armor = item as IArmor;
                segment.Lines.Add(new LeftText($"{armor.GetArmorPoints()} Armor"));
            }
            return segment;
        }

        public ITooltipSection GetStatsSegment(IItem item)
        {
            var segment = new TooltipSection();

            if (item is IEquipment)
            {
                var equipment = item as IEquipment;
                var stats = equipment.GetStats();

                var primaryStats = stats.Where(stat => stat.GetStatType() == StatType.Primary).ToList();
                var secondaryStats = stats.Where(stat => stat.GetStatType() == StatType.Secondary).ToList();
                var tertiaryStats = stats.Where(stat => stat.GetStatType() == StatType.Tertiary).ToList();
                var corruptionStats = stats.Where(stat => stat.GetStatType() == StatType.Corruption).ToList();

                primaryStats.ForEach(stat => segment.Lines.Add(new LeftText(new TooltipText(stat.GetDisplayString(), TooltipColors.Common))));
                secondaryStats.ForEach(stat => segment.Lines.Add(new LeftText(new TooltipText(stat.GetDisplayString(), TooltipColors.Uncommon))));
                tertiaryStats.ForEach(stat => segment.Lines.Add(new LeftText(new TooltipText(stat.GetDisplayString(), TooltipColors.Uncommon))));
                corruptionStats.ForEach(stat => segment.Lines.Add(new LeftText(new TooltipText(stat.GetDisplayString(), TooltipColors.CorruptEffect))));

                // TODO (?):
                // artifact relic slots
                // gem sockets
                // socket bonus


                var durability = equipment.GetDuration();
                if (equipment.HasDurability())
                    segment.Lines.Add(new LeftText($"Durability {durability} / {durability}"));

            }

            // TODO (?):
            // class restrictions

            return segment;
        }

        public ITooltipSection GetEffectsSegment(IItem item)
        {
            var segment = new TooltipSection();

            var effects = item.GetEffects();

            foreach (var effect in effects)
                segment.Lines.Add(new ParagraphLine(new TooltipText($"{effect.GetOrigin()}: {effect.GetEffect()}", effect.GetColor())));

            return segment;
        }

        public ITooltipSection GetFooterSegment(IItem item)
        {
            var segment = new TooltipSection();


            var level = item.GetRequiredLevel();
            if (level != null)
                segment.Lines.Add(new LeftText($"Requires Level {level.GetLevel()}"));

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
                segment.Lines.Add(new LeftText($"Duration: {duration.Value.GetTooltipDisplayString()}"));

            var stack = item.GetMaxStack();
            if (stack > 0)
                segment.Lines.Add(new LeftText($"Max Stack: {stack}"));

            var flavor = item.GetFlavor();
            if (!string.IsNullOrWhiteSpace(flavor))
                segment.Lines.Add(new ParagraphLine(new TooltipText($"\"{flavor}\"", TooltipColors.Flavor)));

            var sellPrice = item.GetSellPrice();
            if (sellPrice != null)
            {
                var gold = sellPrice.GetAmount("g");
                var silver = sellPrice.GetAmount("s");
                var copper = sellPrice.GetAmount("c");
                var sellPriceString = sellPrice.ToString();
                segment.Lines.Add(new CurrencyLine(gold, silver, copper));
            }

            return segment;
        }

        public Image<Rgba32> GetIcon(IItem target) => target.GetIcon().GetImage();
    }
}
