using Awv.Games.WoW.Items;
using Awv.Games.WoW.Levels.Interface;
using System;

namespace Awv.Games.WoW.Levels
{
    public struct ItemLevel : IItemLevel
    {
        /// <summary>
        /// Like other stuff in this class, I don't remember how I got here. Just don't think about it too much.
        /// </summary>
        public const int ArbitraryTurnover = 327;

        public int CoreLevel { get; set; }

        public ItemLevel(int coreLevel)
            : this()
        {
            CoreLevel = coreLevel;
        }

        public int CalculateLevel(ItemRarity rarity)
            => (int)(CoreLevel + Math.Pow(1.4, (int)rarity - (int)ItemRarity.Rare));

        /// <summary>
        /// Calculates the total number of stats for a given rarity at the current item level.
        /// 
        /// I do not remember how I made this. Try not to think about it too hard.
        /// </summary>
        /// <param name="rarity"></param>
        /// <returns></returns>
        public int GetTotalStats(ItemRarity rarity)
        {
            var calculatedLevel = CalculateLevel(rarity);
            return (int)(
                calculatedLevel *
                Math.Pow(1.2, Math.Log(calculatedLevel)) / 8 +
                (calculatedLevel > 75 ?
                    Math.Pow(Math.Max(0, calculatedLevel - 160), Math.Log(calculatedLevel / 35, 11)) * 2 :
                    0));
        }

        public int GetLevel() => CoreLevel;

        /// <summary>
        /// Calculates the DPS for a given rarity at the current item level.
        /// 
        /// Also not too sure about this one. Sorry
        /// </summary>
        /// <param name="rarity"></param>
        /// <returns></returns>
        public int GetDPS(ItemRarity rarity)
        {
            var calculatedLevel = CalculateLevel(rarity);
            return calculatedLevel - (int)(calculatedLevel * (.86 - (calculatedLevel > 160 ? ((double)(calculatedLevel - 160) / ArbitraryTurnover) : 0)));
        }
    }
}
