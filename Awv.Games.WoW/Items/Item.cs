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
        public IGraphic GetIcon() => Icon;
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
        #region Item Methods
        public virtual Image<Rgba32> GenerateTooltip(TooltipGenerator generator, float scale)
        {
            var provider = new ItemTooltipProvider();
            var tt = generator.Generate(provider, this, scale);

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
        public Image<Rgba32> GenerateTooltip(TooltipGenerator generator, float scale, IBrush<Rgba32> background)
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
