using Awv.Games.WoW.Tooltips;
using Awv.Games.WoW.Tooltips.Interface;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Awv.Games.WoW.Graphics
{
    public class TooltipGenerator
    {
        private const string goldsymbol = "!";
        private const string silversymbol = "@";
        private const string coppersymbol = "#";

        public Rgba32 Background { get; set; } = new Rgba32(1, 6, 26);
        public FontFamily FontFamily { get; set; }
        public Image<Rgba32> Emblem { get; set; }
        public int EmblemAnchorY { get; set; }
        public float Scale { get; set; } = 1f;

        public BorderSource Border { get; set; } = new BorderSource();
        public BorderSource Fill { get; set; } = new BorderSource();
        public CurrencySource Currency { get; set; } = new CurrencySource();
        public Rgba32 FillColor { get; set; }

        public Image<Rgba32> Generate(ITooltip tooltip)
        {
            if (FontFamily == null)
                FontFamily = SystemFonts.Families.First(family => family.Name == "Verdana");

            var titleRenderer = new RendererOptions(new Font(FontFamily, Scale * 12f));
            var contentRenderer = new RendererOptions(new Font(FontFamily, Scale * 10f));
            var wrappedContentRenderer = new RendererOptions(contentRenderer.Font);
            var wrapping = new TextGraphicsOptions { WrapTextWidth = wrappedContentRenderer.WrappingWidth };

            var titlePadding = Scale * 96f;

            var title = tooltip.GetTitle();
            var titleSize = TextMeasurer.Measure(title.Text, titleRenderer);

            var tooltipPadding = Border.Padding * Scale;
            var textPadding = new SizeF(12f * Scale, 2f * Scale);

            var lines = tooltip.GetLines().ToList();

            var lineSizes = new List<SizeF>();

            lines.ForEach(line =>
            {
                if (line.LeftText.Type == TooltipTextType.Currency && Currency.Source != null)
                {
                    var regexGold = new Regex(@"\d+g");
                    var regexSilver = new Regex(@"\d{1,2}s");
                    var regexCopper = new Regex(@"\d{1,2}c");

                    var text = line.LeftText.Text;

                    var gold = regexGold.Match(text);
                    if (gold.Success) text = $"{text.Substring(0, gold.Index)}{gold.Value.Substring(0, gold.Value.Length - 1)}  {goldsymbol}  {text.Substring(gold.Index + gold.Value.Length)}";
                    var silver = regexSilver.Match(text);
                    if (silver.Success) text = $"{text.Substring(0, silver.Index)}{silver.Value.Substring(0, silver.Value.Length - 1)}  {silversymbol}{text.Substring(silver.Index + silver.Value.Length)}";
                    var copper = regexCopper.Match(text);
                    if (copper.Success) text = $"{text.Substring(0, copper.Index)}{copper.Value.Substring(0, copper.Value.Length - 1)}  {coppersymbol}  {text.Substring(copper.Index + copper.Value.Length)}";

                    line.LeftText = new TooltipText(text, line.LeftText.Color, line.LeftText.Type);

                }

                switch(line.LeftText.Type)
                {
                    case TooltipTextType.Default:
                    case TooltipTextType.Currency:
                        var leftSize = TextMeasurer.Measure(line.LeftText.Text, contentRenderer);
                        var rightSize = TextMeasurer.Measure(line.RightText.Text, contentRenderer);

                        var lineWidth = leftSize.Width + rightSize.Width + textPadding.Width;
                        var lineHeight = MathF.Max(leftSize.Height, rightSize.Height);

                        lineSizes.Add(new SizeF(lineWidth, lineHeight));
                        break;
                    case TooltipTextType.Paragraph:
                        lineSizes.Add(new SizeF(0, 0));
                        break;
                }
            });

            var width = MathF.Max(titleSize.Width, lineSizes.Max(size => size.Width));
            width += titlePadding;
            wrappedContentRenderer.WrappingWidth = width;
            wrapping.WrapTextWidth = wrappedContentRenderer.WrappingWidth;
            for(var i = 0; i < lines.Count;i++)
            {
                var line = lines[i];

                if (line.LeftText.Type == TooltipTextType.Paragraph)
                {
                    lineSizes[i] = TextMeasurer.Measure(line.LeftText.Text, wrappedContentRenderer);
                }
            }

            var height = titleSize.Height + lineSizes.Sum(line => line.Height) + lineSizes.Count * textPadding.Height;

            width += tooltipPadding.Width;
            height += tooltipPadding.Height;
            height += textPadding.Height * 3f;

            var gen = GenerateBackground((int)width, (int)height);

            gen.Mutate(img =>
            {
                var yoffset = tooltipPadding.Height;
                img.DrawText(title.Text, titleRenderer.Font, title.Color, new PointF(tooltipPadding.Width, yoffset));

                yoffset += titleSize.Height + textPadding.Height;

                for (var i = 0; i < lines.Count;i++)
                {
                    var line = lines[i];
                    var size = lineSizes[i];

                    switch (line.LeftText.Type)
                    {
                        case TooltipTextType.Default:
                            {
                                var left = line.LeftText;
                                var right = line.RightText;
                                var rightSize = TextMeasurer.Measure(right.Text, contentRenderer);

                                img.DrawText(left.Text, contentRenderer.Font, left.Color, new PointF(tooltipPadding.Width, yoffset));
                                img.DrawText(right.Text, contentRenderer.Font, right.Color, new PointF(width - tooltipPadding.Width - rightSize.Width, yoffset));
                                break;
                            }
                        case TooltipTextType.Currency:
                            {
                                var left = line.LeftText;
                                img.DrawText(left.Text, contentRenderer.Font, left.Color, new PointF(tooltipPadding.Width, yoffset));

                                if (Currency.Source != null)
                                {
                                    var gold = Currency.Gold.Clone();
                                    var silver = Currency.Silver.Clone();
                                    var copper = Currency.Copper.Clone();

                                    gold.Mutate(x => x.Resize(new Size((int)(Scale * gold.Width), (int)(Scale * gold.Height))));
                                    silver.Mutate(x => x.Resize(new Size((int)(Scale * silver.Width), (int)(Scale * silver.Height))));
                                    copper.Mutate(x => x.Resize(new Size((int)(Scale * copper.Width), (int)(Scale * copper.Height))));

                                    var tilesizeOffset = (int)(Scale * Currency.TileSize / 4);
                                    if (left.Text.Contains((goldsymbol)))
                                    {
                                        var goldx = TextMeasurer.Measure(left.Text.Substring(0, left.Text.IndexOf(goldsymbol)), contentRenderer).Width - tilesizeOffset;
                                        img.DrawImage(gold, new Point((int)(tooltipPadding.Width + goldx), (int)yoffset - tilesizeOffset), 1f);
                                    }
                                    if (left.Text.Contains((silversymbol)))
                                    {
                                        var silverx = TextMeasurer.Measure(left.Text.Substring(0, left.Text.IndexOf(silversymbol)), contentRenderer).Width - tilesizeOffset;
                                        img.DrawImage(silver, new Point((int)(tooltipPadding.Width + silverx), (int)yoffset - tilesizeOffset), 1f);
                                    }
                                    if (left.Text.Contains((coppersymbol)))
                                    {
                                        var copperx = TextMeasurer.Measure(left.Text.Substring(0, left.Text.IndexOf(coppersymbol)), contentRenderer).Width - tilesizeOffset;
                                        img.DrawImage(copper, new Point((int)(tooltipPadding.Width + copperx), (int)yoffset - tilesizeOffset), 1f);
                                    }
                                }
                                break;
                            }
                        case TooltipTextType.Paragraph:
                            {
                                var left = line.LeftText;
                                img.DrawText(wrapping, left.Text, wrappedContentRenderer.Font, left.Color, new PointF(tooltipPadding.Width, yoffset));
                                break;
                            }
                    }

                    yoffset += size.Height + textPadding.Height;
                }
            });

            return gen;
        }

        public Image<Rgba32> GenerateBackground(int width, int height)
        {
            var tilesize = Border.TileSize;
            var tileSpan = (int)((float)tilesize * 2 * Scale);
            if (width < tileSpan) throw new ArgumentException($"{nameof(width)} must be at least {tileSpan}.");
            if (height < tileSpan) throw new ArgumentException($"{nameof(height)} must be at least {tileSpan}.");


            var bg = new Image<Rgba32>(width, height);
            var fg = new Image<Rgba32>(width, height);
            var layered = new Image<Rgba32>(width, height);

            var scale = new Size((int)(Border.TileSize * Scale), (int)(Border.TileSize * Scale));
            var innerWidth = width - scale.Width * 2;
            var innerHeight = height - scale.Height * 2;

            DrawBorder(bg, Fill, width, height);
            DrawBorder(fg, Border, width, height);

            bg.Mutate(x => x.Fill(FillColor, new Rectangle(scale.Width, scale.Height, innerWidth, innerHeight)));

            layered.Mutate(x => x
                .DrawImage(bg, new Point(0, 0), 1f)
                .DrawImage(fg, new Point(0, 0), 1f)
            );
            return layered;
        }

        private void DrawBorder(Image<Rgba32> target, BorderSource border, int width, int height)
        {
            border.Rescale(Scale);
            var tileSize = new Size(border.TileSize, border.TileSize);
            var scale = new Size((int)(border.TileSize * Scale), (int)(border.TileSize * Scale));
            var innerWidth = width - scale.Width * 2;
            var innerHeight = height - scale.Height * 2;
            var tilesize = border.TileSize;

            target.Mutate(x =>
            {
                x.DrawImage(border.TopLeft, new Point(0, 0), 1f);
                x.DrawImage(border.TopRight, new Point(width - scale.Width, 0), 1f);
                x.DrawImage(border.BottomLeft, new Point(0, height - scale.Height), 1f);
                x.DrawImage(border.BottomRight, new Point(width - scale.Width, height - scale.Height), 1f);

                for (var ix = 0; ix < innerWidth; ix += scale.Width)
                {
                    var scaled = new Size(Math.Min(scale.Width, innerWidth - ix), scale.Height);
                    x.DrawImage(border.Top.Clone(xx => xx.Crop(new Rectangle(Point.Empty, scaled))), new Point(scale.Width + ix, 0), 1f);
                    x.DrawImage(border.Bottom.Clone(xx => xx.Crop(new Rectangle(Point.Empty, scaled))), new Point(scale.Width + ix, height - scale.Height), 1f);
                }

                for (var iy = 0; iy < innerHeight; iy += scale.Height)
                {
                    var scaled = new Size(scale.Width, Math.Min(scale.Height, innerHeight - iy));
                    x.DrawImage(border.Left.Clone(xx => xx.Crop(new Rectangle(Point.Empty, scaled))), new Point(0, scale.Height + iy), 1f);
                    x.DrawImage(border.Right.Clone(xx => xx.Crop(new Rectangle(Point.Empty, scaled))), new Point(width - scale.Width, scale.Height + iy), 1f);
                }
            });

            border.Retile();
        }

    }
}
