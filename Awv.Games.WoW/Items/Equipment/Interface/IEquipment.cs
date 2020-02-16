using Awv.Games.WoW.Items.Effects;
using Awv.Games.WoW.Stats;
using System.Collections.Generic;

namespace Awv.Games.WoW.Items.Equipment.Interface
{
    public interface IEquipment : IItem
    {
        string GetMultiPieceName();
        bool IsMultiEquipment();
        IEnumerable<IWoWStat> GetStats();
        EquipmentType GetEquipmentType();
    }
}
