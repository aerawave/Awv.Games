using Awv.Games.WoW.Tooltips.Text.Interface;
using SixLabors.Fonts;
using SixLabors.Primitives;

namespace Awv.Games.WoW.Tooltips
{
    public class ParagraphLine : IParagraphLine
    {
        public ITooltipText Text { get; set; }
        public ParagraphLine(ITooltipText text)
        {
            Text = text;
        }
        public ParagraphLine(TooltipText text)
            : this(text as ITooltipText) { }
        public ITooltipText GetParagraph() => Text;
        public SizeF Measure(RendererOptions renderer) => TextMeasurer.Measure(GetParagraph().GetText(), renderer);
    }
}
