using Awv.Games.WoW.Items.Effects;
using Awv.Games.WoW.Items.Equipment.Interface;
using Awv.Games.WoW.Tooltips;
using Awv.Games.WoW.Tooltips.Interface;

namespace Awv.Games.WoW.Items.Equipment
{
    public class Armor : Equipment, IArmor
    {
        public decimal ArmorPoints { get; set; }

        public override ITooltipSegment GetCoreSegment()
        {
            var segment = base.GetCoreSegment() as TooltipSegment;

            segment.LeftTexts.Add($"{GetArmorPoints()} Armor");

            return segment;
        }

        public decimal GetArmorPoints() => ArmorPoints;
    }
}
