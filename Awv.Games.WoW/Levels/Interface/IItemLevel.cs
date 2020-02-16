using Awv.Games.WoW.Items;

namespace Awv.Games.WoW.Levels.Interface
{
    public interface IItemLevel
    {
        int GetItemLevel();
        int CalculateLevel(ItemRarity rarity);
        int GetTotalStats(ItemRarity rarity);
        int GetDPS(ItemRarity rarity);
    }
}
