using Awv.Games.Graphics;

namespace ExampleProject.CustomTooltips
{
    public interface ISpell
    {
        IGraphic GetIcon();
        string GetName();
        int GetResourceCost();
        string GetResource();
        /// <summary>
        /// In seconds
        /// </summary>
        decimal GetCastTime();
        /// <summary>
        /// In yards
        /// </summary>
        int GetRange();
        string GetDescription();
    }
}
