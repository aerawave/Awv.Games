using Awv.Games.Currency;
using Awv.Games.Graphics;
using Awv.Games.WoW.Graphics;
using Awv.Games.WoW.Items.Effects;
using Awv.Games.WoW.Items.Equipment.Interface;
using Awv.Games.WoW.Levels;
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
        #region Properties
        public IGraphic Icon { get; set; }
        public ItemRarity Rarity { get; set; }
        public string Name { get; set; }
        public List<string> SpecialItemFlags { get; set; } = new List<string>();
        public string Usage { get; set; }
        public ItemLevel ItemLevel { get; set; }
        public string BindsOn { get; set; }
        public string Uniqueness { get; set; } = null;
        public string ItemType { get; set; }
        public List<UseEffect> UseEffects { get; set; } = new List<UseEffect>();
        public Level RequiredLevel { get; set; }
        public TimeSpan? Duration { get; set; }
        public uint MaxStack { get; set; }
        public string Flavor { get; set; }
        public CurrencyCount SellPrice { get; set; } = new WoWCurrency().GetCurrency(0);
        #endregion
        #region ITooltip Accessors
        public TooltipText GetTitle() => new TooltipText(GetName(), TooltipColors.ToColor(Rarity));
        #endregion
        #region IItem Accessors
        public ItemRarity GetRarity() => Rarity;
        public virtual bool IsCorrupted() => false;
        public string GetName() => Name;
        public IEnumerable<string> GetSpecialItemFlags() => SpecialItemFlags;
        public string GetUsage() => Usage;
        public IItemLevel GetItemLevel() => ItemLevel;
        public string GetBindsOn() => BindsOn;
        public string GetUniqueness() => Uniqueness;
        public string GetItemType() => ItemType;
        public virtual IEnumerable<IEffect> GetEffects() => UseEffects;
        public IPlayerLevel GetRequiredLevel() => RequiredLevel;
        public TimeSpan? GetDuration() => Duration;
        public uint GetMaxStack() => MaxStack;
        public string GetFlavor() => Flavor;
        public CurrencyCount GetSellPrice() => SellPrice;
        #endregion
        #region ITooltip Methods
        public virtual IEnumerable<ITooltipSegment> GetSegments()
        {
            var segments = new List<ITooltipSegment>();
            var core = GetCoreSegment();
            var effects = GetEffectsSegment();

            segments.Add(GetUpperSegment());
            if (core != null) segments.Add(core);
            if (effects != null) segments.Add(effects);
            segments.Add(GetLowerSegment());

            return segments;
        }
        #endregion
        #region Item Methods
        public virtual ITooltipSegment GetCoreSegment() => null;
        public virtual ITooltipSegment GetUpperSegment()
        {
            var equipment = this as IEquipment;
            var list = new List<TooltipText>();

            if (this is IEquipment && equipment.IsMultiEquipment())
            {
                var title = GetTitle();
                list.Add(new TooltipText(equipment.GetMultiPieceName(), title.Color));
            }

            var flags = GetSpecialItemFlags();
            if (flags.Count() > 0)
                list.Add(new TooltipText(string.Join(" ", flags), TooltipColors.Lime));

            var usage = GetUsage();
            if (usage != null)
                list.Add(new TooltipText(usage, TooltipColors.Yellow));

            list.Add(new TooltipText($"Item Level {GetItemLevel().GetLevel()}", TooltipColors.Yellow));

            var bindsOn = GetBindsOn();
            if (!string.IsNullOrWhiteSpace(bindsOn))
                list.Add(new TooltipText(bindsOn, TooltipColors.White));

            var uniqueness = GetUniqueness();
            if (!string.IsNullOrWhiteSpace(uniqueness))
                list.Add(new TooltipText(uniqueness, TooltipColors.White));

            var type = GetItemType();
            if (!string.IsNullOrWhiteSpace(type))
                list.Add(new TooltipText(type, TooltipColors.ItemType));

            return new TooltipSegment { LeftTexts = list };
        }
        public virtual ITooltipSegment GetEffectsSegment()
        {
            var list = new List<TooltipText>();

            var effects = GetEffects();

            foreach (var effect in effects)
                list.Add(new TooltipText($"{effect.GetOrigin()}: {effect.GetEffect()}", effect.GetColor(), TooltipTextType.Paragraph));

            return new TooltipSegment { LeftTexts = list };
        }
        public virtual ITooltipSegment GetLowerSegment()
        {
            var list = new List<TooltipText>();

            var level = GetRequiredLevel();
            if (level != null)
                list.Add(new TooltipText($"Requires Level {level.GetLevel()}", TooltipColors.Common));

            if (this is IEquipment)
            {
                // set name
                // set pieces
                // set effects

                // also Azerite Gear
            }

            var duration = GetDuration();
            if (duration.HasValue)
                list.Add(new TooltipText($"Duration: {duration.Value.GetTooltipDisplayString()}", TooltipColors.Common));

            var stack = GetMaxStack();
            if (stack > 0)
                list.Add(new TooltipText($"Max Stack: {stack}", TooltipColors.Common));

            var flavor = GetFlavor();
            if (!string.IsNullOrWhiteSpace(flavor))
                list.Add(new TooltipText($"\"{flavor}\"", TooltipColors.Flavor, TooltipTextType.Paragraph));

            var sellPrice = GetSellPrice();
            if (sellPrice != null)
            {
                var sellPriceString = sellPrice.ToString();
                if (string.IsNullOrWhiteSpace(sellPriceString)) sellPriceString = "No sell price";
                else sellPriceString = $"Sell Price: {sellPriceString}";
                list.Add(new TooltipText(sellPriceString, TooltipColors.Common, TooltipTextType.Currency));
            }

            return new TooltipSegment { LeftTexts = list };
        }
        public virtual Image<Rgba32> GenerateTooltip(TooltipGenerator generator, float scale)
        {
            var tt = generator.Generate(this, scale);

            if (Icon != null)
            {
                var icon = Icon.GetImage().Clone();

                icon.Mutate(img => img.Resize(new Size((int)(scale * icon.Width), (int)(scale * icon.Height))));
                var ttwic = new Image<Rgba32>(tt.Width + icon.Width, Math.Max(tt.Height, icon.Height));
                ttwic.Mutate(img =>
                {
                    var yoffset = 0;
                    if (generator.Emblem != null)
                        yoffset = (int)(generator.EmblemAnchorY * scale);
                    img.DrawImage(icon, new Point(0, yoffset), 1f);
                    img.DrawImage(tt, new Point(icon.Width, 0), 1f);
                });

                return ttwic;
            }
            else
            {
                return tt;
            }
        }
        public Image<Rgba32> GenerateTooltip(TooltipGenerator generator, float scale, IBrush background)
        {
            var tt = GenerateTooltip(generator, scale);

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
        #endregion
    }
}
