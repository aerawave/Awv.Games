using Awv.Games.Stats;
using Awv.Games.WoW.Stats;
using Awv.Games.WoW.Tooltips;
using Awv.Games.WoW.Tooltips.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Awv.Games.WoW.Items.Equipment
{
    public class Equipment : Item, IEquipment
    {
        public List<IWoWStat> Stats { get; set; } = new List<IWoWStat>();
        public EquipmentType Type { get; set; }

        public bool HasStat(string name) => Stats.Any(stat => stat.GetName() == name);

        public override ITooltipSegment GetUpperSegment()
        {
            var segment = base.GetUpperSegment() as TooltipSegment;
            segment.Level();

            if (Type != null)
            {
                segment.LeftTexts.Add(Type.Slot);
                if (Type.Definition.Name != null)
                    segment.RightTexts.Add(Type.Definition.Name);
            }

            return segment;
        }

        public override ITooltipSegment GetLowerSegment()
        {
            var segment = new TooltipSegment();
            var primaryStats = Stats.Where(stat => stat.GetStatType() == StatType.Primary).ToList();
            var secondaryStats = Stats.Where(stat => stat.GetStatType() == StatType.Secondary).ToList();
            var tertiaryStats = Stats.Where(stat => stat.GetStatType() == StatType.Tertiary).ToList();
            var corruptionStats = Stats.Where(stat => stat.GetStatType() == StatType.Corruption).ToList();

            primaryStats.ForEach(stat => segment.LeftTexts.Add(new TooltipText(stat.GetDisplayString(), TooltipColors.Common)));
            secondaryStats.ForEach(stat => segment.LeftTexts.Add(new TooltipText(stat.GetDisplayString(), TooltipColors.Uncommon)));
            tertiaryStats.ForEach(stat => segment.LeftTexts.Add(new TooltipText(stat.GetDisplayString(), TooltipColors.Uncommon)));
            corruptionStats.ForEach(stat => segment.LeftTexts.Add(new TooltipText(stat.GetDisplayString(), TooltipColors.Corruption)));

            // indestructible ?

            var baseSegment = base.GetLowerSegment() as TooltipSegment;
            baseSegment.Level();

            baseSegment.LeftTexts.ForEach(text => segment.LeftTexts.Add(text));
            baseSegment.RightTexts.ForEach(text => segment.RightTexts.Add(text));

            return segment;
        }

        public EquipmentType GetEquipmentType() => Type;
        public IEnumerable<IWoWStat> GetStats() => Stats;

        protected override bool IsCorrupted() => Stats.Any(stat => stat.GetStatType() == StatType.Corruption);
    }
}
