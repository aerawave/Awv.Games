using Awv.Games.WoW.Items.Effects;
using Awv.Games.WoW.Items.Equipment.Interface;
using Awv.Games.WoW.Tooltips;
using Awv.Games.WoW.Tooltips.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Games.WoW.Items.Equipment
{
    public class Weapon : Equipment, IWeapon
    {
        #region Properties
        public DamageRange DefaultDamage { get; set; } = new DamageRange(0M, 0M);
        public decimal MinimumDamage { get => DefaultDamage.Minimum; set => DefaultDamage.Minimum = value; }
        public decimal MaximumDamage { get => DefaultDamage.Maximum; set => DefaultDamage.Maximum = value; }
        public decimal AttackSpeed { get; set; } = 0M;
        public List<ChanceOnHitEffect> ChanceOnHitEffects { get; set; } = new List<ChanceOnHitEffect>();
        public List<IDamageRange> AdditionalDamage { get; set; } = new List<IDamageRange>();
        public decimal DamagePerSecond => AttackSpeed > 0 ? (GetDamageRanges().Sum(dmg => dmg.GetMinimum() + dmg.GetMaximum()) / (2 * AttackSpeed)) : 0;
        #endregion
        #region IItem Accessors
        public override IEnumerable<IEffect> GetEffects() => ChanceOnHitEffects.Concat(base.GetEffects()).ToArray();
        #endregion
        #region IWeapon Accessors
        public IEnumerable<IDamageRange> GetDamageRanges() => AdditionalDamage.ToArray().Prepend(DefaultDamage);
        public decimal GetAttackSpeed() => AttackSpeed;
        #endregion
    }
}
