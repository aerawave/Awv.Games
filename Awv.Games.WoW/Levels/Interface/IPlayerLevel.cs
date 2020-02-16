using Awv.Games.WoW.Items;

namespace Awv.Games.WoW.Levels.Interface
{
    public interface IPlayerLevel
    {
        int GetLevel();
        ItemLevel GetItemLevel(ItemRarity rarity);
    }
}
