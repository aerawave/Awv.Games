using Awv.Games.WoW.Items;

namespace Awv.Games.WoW.Levels.Interface
{
    public interface IPlayerLevel : ILevel
    {
        IItemLevel GetItemLevel(ItemRarity rarity);
    }
}
