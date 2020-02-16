using Awv.Games.Currency;
using Awv.Games.WoW.Levels.Interface;
using Awv.Games.WoW.Tooltips.Interface;
using System;
using System.Collections.Generic;

namespace Awv.Games.WoW.Items
{
    public interface IItem : ITooltip
    {
        IItemLevel GetItemLevel();
        IPlayerLevel GetRequiredLevel();
        string GetFlavor();
        /// <summary>
        /// Ex: "Champion Equipment"
        /// </summary>
        /// <returns></returns>
        string GetUsage();
        /// <summary>
        /// Ex: "Toy"
        /// </summary>
        /// <returns></returns>
        string GetItemType();
        /// <summary>
        /// Ex: "Unique-Equipped"
        /// </summary>
        /// <returns></returns>
        string GetUniqueness();
        /// <summary>
        /// Ex: "Titanforged" / "Warforged"
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetSpecialItemFlags();
        IEnumerable<string> GetUses();
        IEnumerable<string> GetEquipEffects();
        ItemRarity GetRarity();
        uint GetMaxStack();
        string GetBindsOn();
        TimeSpan? GetDuration();
        CurrencyCount GetSellPrice();
        int GetDurability();
        bool HasDurability();
    }
}
