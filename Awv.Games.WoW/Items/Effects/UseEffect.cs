using Awv.Games.WoW.Tooltips;
using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.WoW.Items.Effects
{
    public class UseEffect : Effect
    {
        public UseEffect(string value, Rgba32 color) : base("Use", value, color) { }
        public UseEffect(string value) : this(value, TooltipColors.Effect) { }
        public UseEffect Corrupt() => new UseEffect(Value, TooltipColors.CorruptEffect);
        public static implicit operator UseEffect(string value) => new UseEffect(value);
    }
}
