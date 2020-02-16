using Awv.Games.WoW.Tooltips;
using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.WoW.Items.Effects
{
    public class EquipEffect : Effect
    {
        public EquipEffect(string value, Rgba32 color) : base("Equip", value, color) { }
        public EquipEffect(string value) : this(value, TooltipColors.Effect) { }
        public EquipEffect Corrupt() => new EquipEffect(Value, TooltipColors.CorruptEffect);
        public static implicit operator EquipEffect(string value) => new EquipEffect(value);
    }
}
