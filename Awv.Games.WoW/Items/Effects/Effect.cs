using Awv.Games.WoW.Tooltips;
using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.WoW.Items.Effects
{
    /// <summary>
    /// Basic customizable <see cref="IEffect"/>.
    /// </summary>
    public class Effect : IEffect
    {
        public virtual string Origin { get; private set; }
        public string Value { get; set; }
        public Rgba32 Color { get; set; }

        public Effect(string origin, string value)
            : this(origin, value, TooltipColors.Effect)
        {
        }

        public Effect(string origin, string value, Rgba32 color)
        {
            Origin = origin;
            Value = value;
            Color = color;
        }

        public string GetEffect() => Value;

        public string GetOrigin() => Origin;
        public Rgba32 GetColor() => Color;

    }
}
