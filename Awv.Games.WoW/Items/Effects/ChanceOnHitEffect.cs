using Awv.Games.WoW.Tooltips;
using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.WoW.Items.Effects
{
    /// <summary>
    /// Used for "Chance on hit: ***" effects.
    /// </summary>
    public class ChanceOnHitEffect : Effect
    {
        private string hitType;
        /// <summary>
        /// The type of hit. Defaults to "Hit". This is for things like "Critical Hit", "Spell Hit", or "Critical Spell Hit".
        /// </summary>
        public string HitType {
            get => hitType ?? "Hit";
            set => hitType = value;
        }

        public override string Origin => $"Chance on {HitType}";

        public ChanceOnHitEffect(string hitType, string value, Rgba32 color) : base("Chance on hit", value, color)
        {
            HitType = hitType;
        }

        public ChanceOnHitEffect(string hitType, string value) : this(hitType, value, TooltipColors.Effect) { }
        public ChanceOnHitEffect(string value) : this(null, value) { }
        public ChanceOnHitEffect(string value, Rgba32 color) : this(null, value, color) { }
        public ChanceOnHitEffect Corrupt() => new ChanceOnHitEffect(hitType, Value, TooltipColors.CorruptEffect);
        public static implicit operator ChanceOnHitEffect(string value) => new ChanceOnHitEffect(value);
    }
}
