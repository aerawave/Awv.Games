using Awv.Games.WoW.Items;
using Awv.Games.WoW.Stats;
using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Games.WoW.Tooltips
{
    public static class TooltipColors
    {
        #region Static Values
        #region Rarity
        public static readonly Rgba32 Poor = new Rgba32(157, 157, 157);
        public static readonly Rgba32 Common = new Rgba32(255, 255, 255);
        public static readonly Rgba32 Uncommon = new Rgba32(30, 255, 0);
        public static readonly Rgba32 Rare = new Rgba32(0, 112, 221);
        public static readonly Rgba32 Epic = new Rgba32(147, 69, 255);
        public static readonly Rgba32 Legendary = new Rgba32(255, 128, 0);
        public static readonly Rgba32 Artifact = new Rgba32(229, 204, 128);
        public static readonly Rgba32 Heirloom = new Rgba32(0, 204, 255);
        #endregion
        #region Other
        //public static readonly Rgba32 Artifact = new Rgba32(153, 136, 86); // i don't know where i got this color??
        public static readonly Rgba32 Token = new Rgba32(0, 112, 144);
        public static readonly Rgba32 Flavor = new Rgba32(255, 209, 0);
        public static readonly Rgba32 ItemType = new Rgba32(102, 187, 255);
        public static readonly Rgba32 Corruption = new Rgba32(132, 97, 185);
        public static readonly Rgba32 Blue = ItemType;
        public static readonly Rgba32 White = Common;
        public static readonly Rgba32 Lime = Uncommon;
        public static readonly Rgba32 Yellow = Flavor;
        #endregion
        #region Stats
        public static readonly Rgba32 PrimaryStat = Common;
        public static readonly Rgba32 SecondaryStat = Uncommon;
        public static readonly Rgba32 TertiaryStat = Uncommon;
        public static readonly Rgba32 CorruptedStat = Corruption;
        #endregion
        #region Effects
        public static readonly Rgba32 Effect = Uncommon;
        public static readonly Rgba32 CorruptEffect = Corruption;
        #endregion
        #region Tooltip fills
        public static readonly Rgba32 FillDefault = new Rgba32(1, 6, 26);
        public static readonly Rgba32 FillCorrupted = new Rgba32(13, 13, 26);
        public static readonly Rgba32 FillAzerite = new Rgba32(15, 20, 28);
        #endregion
        #endregion
        #region Converter extensions
        public static Rgba32 ToColor(this ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Poor: return Poor;
                case ItemRarity.Common: return Common;
                case ItemRarity.Uncommon: return Uncommon;
                case ItemRarity.Rare: return Rare;
                case ItemRarity.Epic: return Epic;
                case ItemRarity.Legendary: return Legendary;
                case ItemRarity.Artifact: return Artifact;
            }
            return Common;
        }
        public static Rgba32 ToColor(this StatType statType)
        {
            switch (statType)
            {
                case StatType.Primary: return PrimaryStat;
                case StatType.Secondary: return SecondaryStat;
                case StatType.Tertiary: return TertiaryStat;
                case StatType.Corruption: return CorruptedStat;
            }
            return Common;
        }
        #endregion
    }
}
