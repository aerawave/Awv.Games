using Awv.Games.WoW.Items.Effects;
using Awv.Games.WoW.Items.Equipment.Interface;
using Awv.Games.WoW.Stats;
using Awv.Games.WoW.Tooltips;
using Awv.Games.WoW.Tooltips.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Games.WoW.Items.Equipment
{
    public class Equipment : Item, IEquipment
    {
        #region Properties
        public string MultiPieceName { get; set; }
        public List<IWoWStat> Stats { get; set; } = new List<IWoWStat>();
        public List<EquipEffect> EquipEffects { get; set; } = new List<EquipEffect>();
        public EquipmentType Type { get; set; }
        public int Durability { get; set; } = 0;
        #endregion
        #region IItem Accessors
        public override bool IsCorrupted() => Stats.Any(stat => stat.GetStatType() == StatType.Corruption);
        public override IEnumerable<IEffect> GetEffects() => EquipEffects.Concat(base.GetEffects()).ToArray();
        #endregion
        #region IEquipment Accessors
        public string GetMultiPieceName() => MultiPieceName;
        public bool IsMultiEquipment() => !string.IsNullOrWhiteSpace(MultiPieceName);
        public IEnumerable<IWoWStat> GetStats() => Stats;
        public EquipmentType GetEquipmentType() => Type;
        public bool HasDurability() => Durability > 0;
        #endregion
        #region Equipment Methods
        public bool HasStat(string name) => Stats.Any(stat => stat.GetName() == name);
        #endregion

    }
}
