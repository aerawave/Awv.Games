using Awv.Games.WoW.Items.Equipment;
using System.Linq;

namespace Awv.Games.WoW.Items
{
    public static class ItemExtensions
    {
        public static bool HasUses(this IItem item)
            => item.GetUses().Count() > 0;

        public static bool HasEquipEffects(this IItem item)
            => item.GetEquipEffects().Count() > 0;

        public static bool HasSpecialItemFlags(this IItem item)
            => item.GetSpecialItemFlags().Count() > 0;

        public static bool HasUniqueness(this IItem item)
            => !string.IsNullOrWhiteSpace(item.GetUniqueness());

        public static bool HasRequiredLevel(this IItem item)
            => item.GetRequiredLevel() != null;

        public static bool HasItemType(this IItem item) => !string.IsNullOrWhiteSpace(item.GetItemType());

        public static bool HasFlavor(this IItem item)
        {
            var flavor = item.GetFlavor();
            return flavor != null && !string.IsNullOrWhiteSpace(flavor);
        }

        public static bool HasSellPrice(this IItem item)
            => item.GetSellPrice() != null;

        public static bool HasBindsOn(this IItem item)
        {
            var bindsOn = item.GetBindsOn();

            return bindsOn != null && !string.IsNullOrWhiteSpace(bindsOn);
        }

        public static bool HasMaxStack(this IItem item)
            => item.GetMaxStack() > 0;

        public static bool HasDuration(this IItem item)
            => item.GetDuration().HasValue;

        public static bool HasStat(this IEquipment item, string name)
            => item.GetStats().Any(stat => stat.GetName() == name);

        public static int GetDisparity(this ItemRarity rarity)
            => (int)rarity - (int)ItemRarity.Rare;
    }
}
