using Awv.Games.Currency;
using Awv.Games.Graphics;
using Awv.Games.WoW.Items.Effects;
using Awv.Games.WoW.Levels.Interface;
using System;
using System.Collections.Generic;

namespace Awv.Games.WoW.Items
{
    public interface IItem
    {
        IGraphic GetIcon();
        ItemRarity GetRarity();
        bool IsCorrupted();
        string GetName();
        /// <summary>
        /// Ex: "Titanforged" / "Warforged"
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetSpecialItemFlags();
        /// <summary>
        /// Ex: "Champion Equipment"
        /// </summary>
        /// <returns></returns>
        string GetUsage();
        IItemLevel GetItemLevel();
        string GetBindsOn();
        /// <summary>
        /// Ex: "Unique-Equipped"
        /// </summary>
        /// <returns></returns>
        string GetUniqueness();
        /// <summary>
        /// Ex: "Toy"
        /// </summary>
        /// <returns></returns>
        string GetItemType();
        /// <summary>
        /// This includes things like "Use:" "Chance on hit:" and "Equip:"
        /// </summary>
        /// <returns></returns>
        IEnumerable<IEffect> GetEffects();
        IPlayerLevel GetRequiredLevel();
        TimeSpan? GetDuration();
        uint GetMaxStack();
        string GetFlavor();
        CurrencyCount GetSellPrice();
    }
}
