using Awv.Games.WoW.Stats;
using System.Collections.Generic;

namespace Awv.Games.WoW.Items.Equipment.Interface
{
    /// <summary>
    /// An interface for an <see cref="IItem"/> which can be equipped.
    /// </summary>
    public interface IEquipment : IItem
    {
        string GetMultiPieceName();
        bool IsMultiEquipment();
        IEnumerable<IWoWStat> GetStats();
        EquipmentType GetEquipmentType();
        bool HasDurability();
    }
}
