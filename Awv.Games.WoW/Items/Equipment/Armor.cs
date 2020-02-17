using Awv.Games.WoW.Items.Equipment.Interface;
using Awv.Games.WoW.Tooltips;
using Awv.Games.WoW.Tooltips.Interface;

namespace Awv.Games.WoW.Items.Equipment
{
    public class Armor : Equipment, IArmor
    {
        #region Properties
        public decimal ArmorPoints { get; set; }
        #endregion
        #region IArmor Accessors
        public decimal GetArmorPoints() => ArmorPoints;
        #endregion
        #region Item Methods
        public override ITooltipSegment GetCoreSegment()
        {
            var segment = base.GetCoreSegment() as TooltipSegment;

            segment.LeftTexts.Add($"{GetArmorPoints()} Armor");

            return segment;
        }
        #endregion
    }
}
