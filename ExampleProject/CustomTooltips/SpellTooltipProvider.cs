using Awv.Games.WoW.Tooltips;
using Awv.Games.WoW.Tooltips.Interface;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;

namespace ExampleProject.CustomTooltips
{
    public class SpellTooltipProvider : ITooltipProvider<ISpell>
    {
        public bool DrawIcon { get; set; } = true;

        public bool ShouldDrawIcon(ISpell spell) => DrawIcon;
        public Image<Rgba32> GetIcon(ISpell spell) => spell.GetIcon().GetImage();

        public TooltipText GetTitle(ISpell spell) => new TooltipText(spell.GetName(), TooltipColors.White);
        public IEnumerable<ITooltipSection> GetSections(ISpell spell)
        {
            var sections = new List<ITooltipSection>();

            var section = new TooltipSection();

            var resource = $"{spell.GetResourceCost()} {spell.GetResource()}";
            var range = $"{spell.GetRange()} yd range";
            var castTime = $"{spell.GetCastTime().ToString("N2")} sec cast";

            section.Lines.Add(new LeftText(resource) + new RightText(range));
            section.Lines.Add(new LeftText(castTime));
            section.Lines.Add(new ParagraphLine(new TooltipText(spell.GetDescription(), TooltipColors.Flavor)));

            sections.Add(section);

            return sections;
        }
    }
}
