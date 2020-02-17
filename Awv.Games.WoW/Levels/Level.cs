using Awv.Games.WoW.Items;
using Awv.Games.WoW.Levels.Interface;
using System;

namespace Awv.Games.WoW.Levels
{
    public struct Level : IPlayerLevel
    {
        public int Value { get; set; }

        public Level(int level)
            : this()
        {
            Value = level;
        }

        public int GetLevel() => Value;

        /// <summary>
        /// Really dumb method for calculating an <see cref="IItemLevel"/> based on the current <see cref="Value"/> and a given <paramref name="rarity"/>. There's not a great deal of logic to these maths, I just tried to make them match wowhead as closely as possible.
        /// </summary>
        /// <param name="rarity">The rarity to calculate the <see cref="IItemLevel"/> for</param>
        /// <returns>The calculated <see cref="IItemLevel"/></returns>
        public IItemLevel GetItemLevel(ItemRarity rarity)
        {
            var level = Value;
            var itemLevel = new ItemLevel { CoreLevel = 5 };
            
            // pre-expansion

            var bracket = 60;
            var bracketMultiplier = 1d;
            var rarityMultiplier = ((int)rarity + 1) * .27d;
            itemLevel.CoreLevel += (int)(bracketMultiplier * Math.Min(level, bracket) * rarityMultiplier);

            if (level > bracket) // tbc / wotlk
            {
                level -= bracket;
                bracket = 20;
                bracketMultiplier = 1.75;
                itemLevel.CoreLevel += (int)(bracketMultiplier * Math.Min(level, bracket) * rarityMultiplier);

                if (level > bracket) // cata / mop
                {
                    level -= bracket;
                    bracket = 10;
                    bracketMultiplier = 1.6;
                    itemLevel.CoreLevel += (int)(bracketMultiplier * Math.Min(level, bracket) * rarityMultiplier);

                    if (level > bracket) // wod
                    {
                        level -= bracket;
                        bracket = 10;
                        bracketMultiplier = 2.2;
                        itemLevel.CoreLevel += (int)(bracketMultiplier * Math.Min(level, bracket) * rarityMultiplier);

                        if (level > bracket) // legion
                        {
                            level -= bracket;
                            bracket = 10;
                            bracketMultiplier = 2.2;
                            itemLevel.CoreLevel += (int)(bracketMultiplier * Math.Min(level, bracket) * rarityMultiplier);

                            if (level > bracket) // bfa
                            {
                                level -= bracket;
                                bracket = 10;
                                bracketMultiplier = 14;
                                itemLevel.CoreLevel += (int)(bracketMultiplier * Math.Min(level, bracket) * rarityMultiplier);
                            }
                        }
                    }
                }
            }

            return itemLevel;
        }

        public static implicit operator int(Level playerLevel)
            => playerLevel.Value;

        public static implicit operator Level(int level)
            => new Level(level);

        public override string ToString() => Value.ToString();

        public string ToString(IFormatProvider provider) => Value.ToString(provider);
        public string ToString(string format) => Value.ToString(format);
        public string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
    }
}
