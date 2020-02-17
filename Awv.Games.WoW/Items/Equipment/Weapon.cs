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
        public DamageRange DefaultDamage { get; set; } = new DamageRange(0M, 0M);
        public decimal MinimumDamage { get => DefaultDamage.Minimum; set => DefaultDamage.Minimum = value; }
        public decimal MaximumDamage { get => DefaultDamage.Maximum; set => DefaultDamage.Maximum = value; }
        public decimal AttackSpeed { get; set; } = 0M;
        public List<ChanceOnHitEffect> ChanceOnHitEffects { get; set; } = new List<ChanceOnHitEffect>();

        public List<IDamageRange> AdditionalDamage { get; set; } = new List<IDamageRange>();

        public decimal DamagePerSecond => AttackSpeed > 0 ? (GetDamageRanges().Sum(dmg => dmg.GetMinimum() + dmg.GetMaximum()) / (2 * AttackSpeed)) : 0;

        public override ITooltipSegment GetCoreSegment()
        {
            var segment = base.GetCoreSegment() as TooltipSegment;

            segment.LeftTexts.Add(DefaultDamage.GetDisplayString());
            segment.RightTexts.Add($"Speed {AttackSpeed.ToString("N2")}");

            foreach (var damage in AdditionalDamage)
                segment.LeftTexts.Add($"+ {damage.GetDisplayString()}");

            segment.LeftTexts.Add($"({DamagePerSecond.ToString("N1")} damage per second)");

            return segment;
        }

        public override IEnumerable<IEffect> GetEffects() => ChanceOnHitEffects.Concat(base.GetEffects()).ToArray();

        public decimal GetAttackSpeed() => AttackSpeed;

        public IEnumerable<IDamageRange> GetDamageRanges()
            => AdditionalDamage.ToArray().Prepend(DefaultDamage);
    }
}
