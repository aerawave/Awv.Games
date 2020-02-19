using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;

namespace Awv.Games.WoW.Tooltips.Interface
{
    public interface ITooltipProvider
    {
        bool ShouldDrawIcon();
        Image<Rgba32> GetIcon();
        TooltipText GetTitle();
        IEnumerable<ITooltipSection> GetSegments();
    }

    public interface ITooltipProvider<TData>
    {
        bool ShouldDrawIcon(TData target);
        Image<Rgba32> GetIcon(TData target);
        TooltipText GetTitle(TData target);
        IEnumerable<ITooltipSection> GetSegments(TData target);
    }
}
