using System.Collections.Generic;

namespace Awv.Games.WoW.Items.Equipment.Interface
{
    /// <summary>
    /// An interface for a piece of <see cref="IEquipment"/> which can be equipped in a weapon slot.
    /// </summary>
    public interface IWeapon : IEquipment
    {
        IEnumerable<IDamageRange> GetDamageRanges();
        decimal GetAttackSpeed();
    }
}
