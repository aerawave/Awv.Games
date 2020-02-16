using Awv.Games.WoW.Items;
using Awv.Games.WoW.Levels.Interface;
using System;

namespace Awv.Games.WoW.Levels
{
    public struct PlayerLevel : IPlayerLevel
    {
        public int Level { get; set; }

        public PlayerLevel(int level)
            : this()
        {
            Level = level;
        }

        public int GetLevel() => Level;

        public ItemLevel GetItemLevel(ItemRarity rarity)
        {
            var lvl = Level;
            var ilvl = new ItemLevel { CoreLevel = 5 };

            int bracket = 60;
            double multip = 1;
            double m = ((int)rarity + 1) * .27d;
            ilvl.CoreLevel += (int)(multip * Math.Min(lvl, bracket) * m);

            if (lvl > bracket) // tbc / wotlk
            {
                lvl -= bracket;
                bracket = 20;
                multip = 1.75;
                ilvl.CoreLevel += (int)(multip * Math.Min(lvl, bracket) * m);

                if (lvl > bracket) // cata / mop
                {
                    lvl -= bracket;
                    bracket = 10;
                    multip = 1.6;
                    ilvl.CoreLevel += (int)(multip * Math.Min(lvl, bracket) * m);

                    if (lvl > bracket) // wod
                    {
                        lvl -= bracket;
                        bracket = 10;
                        multip = 2.2;
                        ilvl.CoreLevel += (int)(multip * Math.Min(lvl, bracket) * m);

                        if (lvl > bracket) // legion
                        {
                            lvl -= bracket;
                            bracket = 10;
                            multip = 2.2;
                            ilvl.CoreLevel += (int)(multip * Math.Min(lvl, bracket) * m);

                            if (lvl > bracket) // bfa
                            {
                                lvl -= bracket;
                                bracket = 10;
                                multip = 14;
                                ilvl.CoreLevel += (int)(multip * Math.Min(lvl, bracket) * m);
                            }
                        }
                    }
                }
            }

            return ilvl;
        }

        public static implicit operator int(PlayerLevel pl)
            => pl.Level;

        public static implicit operator PlayerLevel(int level)
            => new PlayerLevel(level);

        public override string ToString()
            => Level.ToString();
    }
}
