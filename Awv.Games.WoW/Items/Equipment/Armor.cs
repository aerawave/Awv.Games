using Awv.Games.WoW.Items.Equipment.Interface;

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
    }
}
