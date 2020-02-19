using Awv.Games.WoW.Tooltips;
using Awv.Games.WoW.Tooltips.Interface;
using Awv.Games.WoW.Tooltips.Text;
using Awv.Games.WoW.Tooltips.Text.Interface;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public BorderSource Border { get; set; } = new BorderSource();
        public BorderSource Fill { get; set; } = new BorderSource();
        public CurrencySource Currency { get; set; } = new CurrencySource();
        public Rgba32 FillColor { get; set; }

        public Image<Rgba32> Generate<TTarget>(ITooltipProvider<TTarget> provider, TTarget target, float scale)
            => Generate(provider.ShouldDrawIcon(target) ? provider.GetIcon(target) : null, provider.GetTitle(target), provider.GetSegments(target).ToArray(), scale);

        public Image<Rgba32> Generate(ITooltipProvider provider, float scale)
            => Generate(provider.ShouldDrawIcon() ? provider.GetIcon() : null, provider.GetTitle(), provider.GetSegments().ToArray(), scale);

        private Image<Rgba32> Generate(Image<Rgba32> icon, TooltipText title, ITooltipSection[] segments, float scale)
        {
            var linesQuery = segments.SelectMany(segment => segment.GetLines());
            if (FontFamily == null)
                FontFamily = SystemFonts.Families.First(family => family.Name == "Verdana");

            var titleRenderer = new RendererOptions(new Font(FontFamily, scale * 12f));
            var contentRenderer = new RendererOptions(new Font(FontFamily, scale * 10f));
            var wrappedContentRenderer = new RendererOptions(contentRenderer.Font);
            var wrapping = new TextGraphicsOptions { WrapTextWidth = wrappedContentRenderer.WrappingWidth };

            var titlePadding = scale * 96f;

            var titleSize = TextMeasurer.Measure(title.Text, titleRenderer);

            var tooltipPadding = Border.Padding * scale;
            var textPadding = new SizeF(12f * scale, 2f * scale);

            var lineSizes = new List<SizeF>();


            var lines = linesQuery.Select(line =>
            {
                if (line is ICurrencyLine && Currency.Source != null)
                {
                    var currency = line as ICurrencyLine;
                    var sb = new StringBuilder();
                    var gold = currency.GetGold();
                    var silver = currency.GetSilver();
                    var copper = currency.GetCopper();


                    sb.Append("Sell Price: ");
                    var total = copper + (silver * 100) + (gold * 10000);
                    if (gold > 0) sb.Append($"{gold}  {goldsymbol}   ");
                    if (silver > 0) sb.Append($"{silver}  {silversymbol} ");
                    if (copper > 0) sb.Append($"{copper}  {coppersymbol}");
                    if (total <= 0) sb.Clear().Append("No sell price");

                    return new CurrencyLine(sb.ToString().Trim()) as ITooltipLine;
                } else
                {
                    return line;
                }
            }).ToArray();
            foreach(var line in lines)
            {
                if (line is IParagraphLine)
                {
                    lineSizes.Add(new SizeF(0, 0));
                } else
                {
                    var leftSize = (line as ILeftText)?.Measure(contentRenderer) ?? SizeF.Empty;
                    var rightSize = (line as IRightText)?.Measure(contentRenderer) ?? SizeF.Empty;

                    var lineWidth = leftSize.Width + rightSize.Width + textPadding.Width;
                    var lineHeight = MathF.Max(leftSize.Height, rightSize.Height);

                    lineSizes.Add(new SizeF(lineWidth, lineHeight));
                }
            }

            var width = MathF.Max(titleSize.Width, lineSizes.Max(size => size.Width));
            width += titlePadding;
            wrappedContentRenderer.WrappingWidth = width;
            wrapping.WrapTextWidth = wrappedContentRenderer.WrappingWidth;

            foreach(var entry in lines.Select((line, i) => new { Line = line, Index = i })) {
                var line = entry.Line;

                if (line is IParagraphLine)
                    lineSizes[entry.Index] = line.Measure(wrappedContentRenderer);
            }

            var height = titleSize.Height + lineSizes.Sum(line => line.Height) + lineSizes.Count * textPadding.Height;

            width += tooltipPadding.Width * 2;
            height += tooltipPadding.Height * 2;

            var tooltip = GenerateBackground((int)width, (int)height, scale);

            tooltip.Mutate(img =>
            {
                var yoffset = tooltipPadding.Height;
                if (Emblem != null)
                    yoffset += EmblemAnchorY * scale;

                img.DrawText(title.Text, titleRenderer.Font, title.Color, new PointF(tooltipPadding.Width, yoffset));

                yoffset += titleSize.Height + textPadding.Height;

                foreach (var entry in lines.Zip(lineSizes).Select(x => new { Line = x.First, Size = x.Second }))
                {
                    var line = entry.Line;
                    var size = entry.Size;

                    if (line is IParagraphLine)
                    {
                        var paragraph = (line as IParagraphLine).GetParagraph();

                        img.DrawText(wrapping, paragraph.GetText(), wrappedContentRenderer.Font, paragraph.GetColor(), new PointF(tooltipPadding.Width, yoffset));
                    } else if (line is CurrencyLine)
                    {
                        var text = (line as CurrencyLine?)?.GetLeftText();
                        var value = text?.GetText();
                        img.DrawText(value, contentRenderer.Font, text.GetColor(), new PointF(tooltipPadding.Width, yoffset));

                        if (Currency.Source != null)
                        {
                            //Currency.Tile();
                            //Currency.Scale(scale);
                            var gold = Currency.Gold.Clone();
                            var silver = Currency.Silver.Clone();
                            var copper = Currency.Copper.Clone();



                            gold.Mutate(x => x.Resize(new Size((int)(scale * gold.Width), (int)(scale * gold.Height))));
                            silver.Mutate(x => x.Resize(new Size((int)(scale * silver.Width), (int)(scale * silver.Height))));
                            copper.Mutate(x => x.Resize(new Size((int)(scale * copper.Width), (int)(scale * copper.Height))));

                            var tilesizeOffset = (int)(scale * Currency.TileSize / 4);
                            if (value.Contains((goldsymbol)))
                            {
                                var goldx = TextMeasurer.Measure(value.Substring(0, value.IndexOf(goldsymbol)), contentRenderer).Width - tilesizeOffset;
                                img.DrawImage(gold, new Point((int)(tooltipPadding.Width + goldx), (int)yoffset - tilesizeOffset), 1f);
                            }
                            if (value.Contains((silversymbol)))
                            {
                                var silverx = TextMeasurer.Measure(value.Substring(0, value.IndexOf(silversymbol)), contentRenderer).Width - tilesizeOffset;
                                img.DrawImage(silver, new Point((int)(tooltipPadding.Width + silverx), (int)yoffset - tilesizeOffset), 1f);
                            }
                            if (value.Contains((coppersymbol)))
                            {
                                var copperx = TextMeasurer.Measure(value.Substring(0, value.IndexOf(coppersymbol)), contentRenderer).Width - tilesizeOffset;
                                img.DrawImage(copper, new Point((int)(tooltipPadding.Width + copperx), (int)yoffset - tilesizeOffset), 1f);
                            }
                        }
                    } else if (line is ILeftText || line is IRightText)
                    {
                        var leftText = (line as ILeftText)?.GetLeftText();
                        var rightText = (line as IRightText)?.GetRightText();

                        if (leftText != null)
                        {
                            img.DrawText(leftText.GetText(), contentRenderer.Font, leftText.GetColor(), new PointF(tooltipPadding.Width, yoffset));
                        }
                        if (rightText != null)
                        {
                            var rightSize = rightText.Measure(contentRenderer);
                            img.DrawText(rightText.GetText(), contentRenderer.Font, rightText.GetColor(), new PointF(width - tooltipPadding.Width - rightSize.Width, yoffset));
                        }
                    }

                    yoffset += size.Height + textPadding.Height;
                }
            });

            if (icon != null)
            {

                icon.Mutate(img => img.Resize(new Size((int)(scale * icon.Width), (int)(scale * icon.Height))));
                var tooltipWithIcon = new Image<Rgba32>(tooltip.Width + icon.Width, Math.Max(tooltip.Height, icon.Height));
                tooltipWithIcon.Mutate(img =>
                {
                    var yoffset = 0;
                    if (Emblem != null)
                        yoffset = (int)(EmblemAnchorY * scale);
                    img.DrawImage(icon, new Point(0, yoffset), 1f);
                    img.DrawImage(tooltip, new Point(icon.Width, 0), 1f);
                });

                return tooltipWithIcon;
            } else
            {
                return tooltip;
            }
        }

        public Image<Rgba32> GenerateBackground(int width, int height, float scale)
        {
            var tilesize = Border.TileSize;
            var tileSpan = (int)((float)tilesize * 2 * scale);
            width = Math.Max(width, tileSpan);
            height = Math.Max(height, tileSpan);

            var yoffset = 0f;
            if (Emblem != null)
                yoffset = EmblemAnchorY * scale;

            var yoffseti = (int)yoffset;
            var bg = new Image<Rgba32>(width, height + yoffseti);
            var fg = new Image<Rgba32>(width, height + yoffseti);
            var layered = new Image<Rgba32>(width, height + yoffseti);

            var scaled = new Size((int)(Border.TileSize * scale), (int)(Border.TileSize * scale));
            var innerWidth = width - scaled.Width * 2;
            var innerHeight = height - scaled.Height * 2;

            DrawBorder(bg, Fill, width, height, scale);
            DrawBorder(fg, Border, width, height, scale);

            bg.Mutate(x => x.Fill(FillColor, new Rectangle(scaled.Width, scaled.Height, innerWidth, innerHeight)));

            layered.Mutate(x =>
            {
                x.DrawImage(bg, new Point(0, yoffseti), 1f);
                x.DrawImage(fg, new Point(0, yoffseti), 1f);
                if (Emblem != null)
                {
                    var emblem = Emblem.Clone(img => img.Resize((int)(Emblem.Width * scale), (int)(Emblem.Height * scale)));
                    x.DrawImage(emblem, new Point(width / 2 - emblem.Width / 2, 0), 1f);
                }
            });
            return layered;
        }

        private void DrawBorder(Image<Rgba32> target, BorderSource border, int width, int height, float scale)
        {
            border.Scale(scale);
            var tileSize = new Size(border.TileSize, border.TileSize);
            var scaled = new Size((int)(border.TileSize * scale), (int)(border.TileSize * scale));
            var innerWidth = width - scaled.Width * 2;
            var innerHeight = height - scaled.Height * 2;
            var tilesize = border.TileSize;

            target.Mutate(x =>
            {
                x.DrawImage(border.TopLeft, new Point(0, 0), 1f);
                x.DrawImage(border.TopRight, new Point(width - scaled.Width, 0), 1f);
                x.DrawImage(border.BottomLeft, new Point(0, height - scaled.Height), 1f);
                x.DrawImage(border.BottomRight, new Point(width - scaled.Width, height - scaled.Height), 1f);

                for (var ix = 0; ix < innerWidth; ix += scaled.Width)
                {
                    var size = new Size(Math.Min(scaled.Width, innerWidth - ix), scaled.Height);
                    x.DrawImage(border.Top.Clone(xx => xx.Crop(new Rectangle(Point.Empty, size))), new Point(scaled.Width + ix, 0), 1f);
                    x.DrawImage(border.Bottom.Clone(xx => xx.Crop(new Rectangle(Point.Empty, size))), new Point(scaled.Width + ix, height - scaled.Height), 1f);
                }

                for (var iy = 0; iy < innerHeight; iy += scaled.Height)
                {
                    var size = new Size(scaled.Width, Math.Min(scaled.Height, innerHeight - iy));
                    x.DrawImage(border.Left.Clone(xx => xx.Crop(new Rectangle(Point.Empty, size))), new Point(0, scaled.Height + iy), 1f);
                    x.DrawImage(border.Right.Clone(xx => xx.Crop(new Rectangle(Point.Empty, size))), new Point(width - scaled.Width, scaled.Height + iy), 1f);
                }
            });

            border.Tile();
        }

    }
}
