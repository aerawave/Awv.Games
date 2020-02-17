using Awv.Games.Currency;
using Awv.Games.WoW.Items.Effects;
using Awv.Games.WoW.Levels.Interface;
using Awv.Games.WoW.Tooltips.Interface;
using System;
using System.Collections.Generic;

namespace Awv.Games.WoW.Items
{
    public interface IItem : ITooltip
    {
        string GetName();
        IItemLevel GetItemLevel();
        string GetBindsOn();
        /// <summary>
        /// Ex: "Unique-Equipped"
        /// </summary>
        /// <returns></returns>
        string GetUniqueness();
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
        /// Ex: "Titanforged" / "Warforged"
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetSpecialItemFlags();
        /// <summary>
        /// This includes things like "Use:" "Chance on hit:" and "Equip:"
        /// </summary>
        /// <returns></returns>
        IEnumerable<IEffect> GetEffects();
        ItemRarity GetRarity();
        uint GetMaxStack();
        TimeSpan? GetDuration();
        CurrencyCount GetSellPrice();
        int GetDurability();
        bool HasDurability();
        bool IsCorrupted();
    }
}
