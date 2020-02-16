using Awv.Games.WoW.Tooltips;
using Awv.Games.WoW.Tooltips.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awv.Games.WoW.Items.Equipment
{
    public class Weapon : Equipment, IWeapon
    {
        public decimal MinimumDamage { get; set; } = 0M;
        public decimal MaximumDamage { get; set; } = 0M;
        public decimal AttackSpeed { get; set; } = 0M;

        public decimal DamagePerSecond => AttackSpeed > 0 ? ((MinimumDamage + MaximumDamage) / (2 * AttackSpeed)) : 0;

        public override ITooltipSegment GetCoreSegment()
        {
            var segment = new TooltipSegment();

            segment.LeftTexts.Add($"{MinimumDamage} - {MaximumDamage} Damage");
            segment.RightTexts.Add($"Speed {AttackSpeed.ToString("N2")}");

            segment.LeftTexts.Add($"({DamagePerSecond.ToString("N1")} damage per second)");

            return segment;
        }

        public decimal GetMinimumDamage() => MinimumDamage;

        public decimal GetMaximumDamage() => MaximumDamage;

        public decimal GetAttackSpeed() => AttackSpeed;

    }
}
