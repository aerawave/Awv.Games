using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.WoW.Items.Effects
{
    public interface IEffect
    {
        string GetOrigin();
        string GetEffect();
        Rgba32 GetColor();
    }
}
