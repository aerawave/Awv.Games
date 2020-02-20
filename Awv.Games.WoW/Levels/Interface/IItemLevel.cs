using Awv.Games.WoW.Items;

namespace Awv.Games.WoW.Levels.Interface
{
    public interface IItemLevel : ILevel
    {
        int CalculateLevel(ItemRarity rarity);
        int GetTotalStats(ItemRarity rarity);
        decimal GetDPS(ItemRarity rarity);
    }
}
