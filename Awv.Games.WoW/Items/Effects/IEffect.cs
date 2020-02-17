using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.WoW.Items.Effects
{
    /// <summary>
    /// An item effect interface. For things like "Equip: ***" or "Use: ***"
    /// </summary>
    public interface IEffect
    {
        string GetOrigin();
        string GetEffect();
        Rgba32 GetColor();
    }
}
