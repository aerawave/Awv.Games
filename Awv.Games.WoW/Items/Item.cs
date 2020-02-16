using Awv.Games.Currency;
using Awv.Games.Graphics;
using Awv.Games.WoW.Graphics;
using Awv.Games.WoW.Levels.Interface;
using Awv.Games.WoW.Tooltips;
using Awv.Games.WoW.Tooltips.Interface;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Games.WoW.Items
{
    public class Item : IItem
    {
        public string Name { get; set; }
        public IItemLevel ItemLevel { get; set; }
        public IPlayerLevel RequiredLevel { get; set; }
        public ItemRarity Rarity { get; set; }
        public string Flavor { get; set; }
        public string Usage { get; set; }
        public string ItemType { get; set; }
        public string BindsOn { get; set; }
        public string Uniqueness { get; set; } = null;
        public List<string> SpecialItemFlags { get; set; } = new List<string>();
        public List<string> Uses { get; set; } = new List<string>();
        public List<string> EquipEffects { get; set; } = new List<string>();
        // icon
        public IGraphic Icon { get; set; }
        public bool HasIcon => Icon != null;
        public CurrencyCount SellPrice { get; set; } = new WoWCurrency().GetCurrency(0);
        public uint MaxStack { get; set; }
        public string Corruption { get; set; }
        public int Durability { get; set; } = 0;
        public TimeSpan? Duration { get; set; }

        public string GetBindsOn() => BindsOn;
        public TimeSpan? GetDuration() => Duration;
        public IEnumerable<string> GetEquipEffects() => EquipEffects;
        public string GetFlavor() => Flavor;
        public IItemLevel GetItemLevel() => ItemLevel;
        public string GetItemType() => ItemType;
        public uint GetMaxStack() => MaxStack;
        public ItemRarity GetRarity() => Rarity;
        public IPlayerLevel GetRequiredLevel() => RequiredLevel;
        public bool HasDurability() => Durability > 0;


        public virtual IEnumerable<ITooltipSegment> GetSegments()
        {
            var segments = new List<ITooltipSegment>();
            var core = GetCoreSegment();

            segments.Add(GetUpperSegment());
            if (core != null) segments.Add(core);
            segments.Add(GetLowerSegment());

            return segments;
        }

        public IEnumerable<string> GetSpecialItemFlags() => SpecialItemFlags;

        public TooltipText GetTitle() => new TooltipText(Name, TooltipColors.ToColor(Rarity));

        public string GetUniqueness() => Uniqueness;

        public string GetUsage() => Usage;

        public IEnumerable<string> GetUses() => Uses;
        public CurrencyCount GetSellPrice() => SellPrice;
        public int GetDurability() => Durability;

        public virtual ITooltipSegment GetCoreSegment() => null;

        public virtual ITooltipSegment GetUpperSegment()
        {
            var list = new List<TooltipText>();

            if (this.HasSpecialItemFlags()) list.Add(new TooltipText(string.Join(" ", GetSpecialItemFlags()), TooltipColors.Uncommon));

            var usage = GetUsage();
            if (usage != null) list.Add(new TooltipText(usage, TooltipColors.Flavor));

            list.Add(new TooltipText($"Item Level {GetItemLevel().GetItemLevel()}", TooltipColors.Flavor));

            if (this.HasBindsOn()) list.Add(new TooltipText(GetBindsOn(), TooltipColors.Common));
            if (this.HasUniqueness()) list.Add(new TooltipText(GetUniqueness(), TooltipColors.Common));
            if (this.HasItemType()) list.Add(new TooltipText(GetItemType(), TooltipColors.ItemType));

            return new TooltipSegment { LeftTexts = list };
        }

        public virtual ITooltipSegment GetLowerSegment()
        {
            var list = new List<TooltipText>();

            var equipColor = IsCorrupted() ? TooltipColors.Corruption : TooltipColors.Uncommon;


            if (this.HasUses()) GetUses().ToList().ForEach(use => list.Add(new TooltipText($"Use: {use}", TooltipColors.Uncommon, TooltipTextType.Paragraph)));
            if (this.HasEquipEffects()) GetEquipEffects().ToList().ForEach(equip => list.Add(new TooltipText($"Equip: {equip}", equipColor, TooltipTextType.Paragraph)));
            if (this.HasRequiredLevel()) list.Add(new TooltipText($"Requires Level {GetRequiredLevel().GetLevel()}", TooltipColors.Common));

            if (HasDurability()) list.Add($"Durability {GetDurability()} / {GetDurability()}");

            if (this.HasDuration()) list.Add(new TooltipText($"Duration: {GetDuration().Value.GetTooltipDisplayString()}", TooltipColors.Common));
            if (this.HasMaxStack()) list.Add(new TooltipText($"Max Stack: {GetMaxStack()}", TooltipColors.Common));

            if (this.HasFlavor()) list.Add(new TooltipText($"\"{GetFlavor()}\"", TooltipColors.Flavor, TooltipTextType.Paragraph));

            if (this.HasSellPrice())
            {
                var sellPrice = GetSellPrice().ToString();
                if (string.IsNullOrWhiteSpace(sellPrice)) sellPrice = "No sell price";
                else sellPrice = $"Sell Price: {sellPrice}";
                list.Add(new TooltipText(sellPrice, TooltipColors.Common, TooltipTextType.Currency));
            }

            return new TooltipSegment { LeftTexts = list };
        }

        public virtual Image<Rgba32> GenerateTooltip(TooltipGenerator generator)
        {
            var tt = generator.Generate(this);

            if (Icon != null)
            {
                var icon = Icon.GetImage().Clone();

                icon.Mutate(img => img.Resize(new Size((int)(generator.Scale * icon.Width), (int)(generator.Scale * icon.Height))));
                var ttwic = new Image<Rgba32>(tt.Width + icon.Width, Math.Max(tt.Height, icon.Height));
                ttwic.Mutate(img =>
                {
                    img.DrawImage(icon, new Point(0, 0), 1f);
                    img.DrawImage(tt, new Point(icon.Width, 0), 1f);
                });

                return ttwic;
            }
            else
            {
                return tt;
            }
        }

        public Image<Rgba32> GenerateTooltip(TooltipGenerator generator, IBrush background)
        {
            var tt = GenerateTooltip(generator);

            if (background != null)
            {
                var ttwbg = new Image<Rgba32>(tt.Width, tt.Height);

                ttwbg.Mutate(img =>
                {
                    img.Fill(background, new RectangleF(0, 0, tt.Width, tt.Height));
                    img.DrawImage(tt, new Point(0, 0), 1f);
                });

                return ttwbg;
            }
            else
            {
                return tt;
            }
        }

        protected virtual bool IsCorrupted() => false;
    }
}
