using Awv.Games.Stats;
using Awv.Games.WoW.Stats;
using System.Collections.Generic;

namespace Awv.Games.WoW.Items.Equipment
{
    public interface IEquipment : IItem
    {
        IEnumerable<IWoWStat> GetStats();
        EquipmentType GetEquipmentType();
    }
}
