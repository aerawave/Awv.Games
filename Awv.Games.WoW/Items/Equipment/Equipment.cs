using Awv.Games.WoW.Items.Effects;
using Awv.Games.WoW.Items.Equipment.Interface;
using Awv.Games.WoW.Stats;
using Awv.Games.WoW.Tooltips;
using Awv.Games.WoW.Tooltips.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Games.WoW.Items.Equipment
{
    public class Equipment : Item, IEquipment
    {
        public string MultiPieceName { get; set; }
        public List<IWoWStat> Stats { get; set; } = new List<IWoWStat>();
        public List<EquipEffect> EquipEffects { get; set; } = new List<EquipEffect>();
        public EquipmentType Type { get; set; }

        public bool HasStat(string name) => Stats.Any(stat => stat.GetName() == name);

        public override IEnumerable<ITooltipSegment> GetSegments()
        {
            var segments = new List<ITooltipSegment>();
            var upper = GetUpperSegment();
            var core = GetCoreSegment();
            var stats = GetStatsSegment();
            var effects = GetEffectsSegment();
            var lower = GetLowerSegment();

            segments.Add(upper);

            segments.Add(core);
            segments.Add(stats);
            segments.Add(effects);

            segments.Add(lower);

            return segments;
        }

        public override ITooltipSegment GetCoreSegment()
        {
            var segment = new TooltipSegment();

            if (Type != null)
                segment.LeftTexts.Add(Type.Slot);
            if (Type?.Definition?.Name != null)
                segment.RightTexts.Add(Type.Definition.Name);

            return segment;
        }

        public virtual ITooltipSegment GetStatsSegment()
        {
            var segment = new TooltipSegment();
            var primaryStats = Stats.Where(stat => stat.GetStatType() == StatType.Primary).ToList();
            var secondaryStats = Stats.Where(stat => stat.GetStatType() == StatType.Secondary).ToList();
            var tertiaryStats = Stats.Where(stat => stat.GetStatType() == StatType.Tertiary).ToList();
            var corruptionStats = Stats.Where(stat => stat.GetStatType() == StatType.Corruption).ToList();

            primaryStats.ForEach(stat => segment.LeftTexts.Add(new TooltipText(stat.GetDisplayString(), TooltipColors.Common)));
            secondaryStats.ForEach(stat => segment.LeftTexts.Add(new TooltipText(stat.GetDisplayString(), TooltipColors.Uncommon)));
            tertiaryStats.ForEach(stat => segment.LeftTexts.Add(new TooltipText(stat.GetDisplayString(), TooltipColors.Uncommon)));
            corruptionStats.ForEach(stat => segment.LeftTexts.Add(new TooltipText(stat.GetDisplayString(), TooltipColors.CorruptEffect)));

            // artifact relic slots

            // gem sockets
            // socket bonus

            var durability = Durability;
            if (HasDurability())
                segment.LeftTexts.Add($"Durability {durability} / {durability}");

            // class restrictions

            return segment;
        }

        public override IEnumerable<IEffect> GetEffects() => EquipEffects.Concat(base.GetEffects()).ToArray();
        public EquipmentType GetEquipmentType() => Type;
        public IEnumerable<IWoWStat> GetStats() => Stats;

        public override bool IsCorrupted() => Stats.Any(stat => stat.GetStatType() == StatType.Corruption);

        public string GetMultiPieceName() => MultiPieceName;

        public bool IsMultiEquipment() => !string.IsNullOrWhiteSpace(MultiPieceName);
    }
}
