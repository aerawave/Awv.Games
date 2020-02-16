using System;
using System.Collections.Generic;
using System.Text;

namespace Awv.Games.WoW.Items.Equipment
{
    public interface IWeapon : IEquipment
    {
        decimal GetMinimumDamage();
        decimal GetMaximumDamage();
        decimal GetAttackSpeed();
    }
}
