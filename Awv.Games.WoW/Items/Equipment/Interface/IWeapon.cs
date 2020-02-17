using Awv.Games.WoW.Items.Effects;
using System.Collections.Generic;

namespace Awv.Games.WoW.Items.Equipment.Interface
{
    public interface IWeapon : IEquipment
    {
        IEnumerable<IDamageRange> GetDamageRanges();
        decimal GetAttackSpeed();
    }
}
