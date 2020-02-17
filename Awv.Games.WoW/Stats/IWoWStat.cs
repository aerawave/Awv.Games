using Awv.Games.Stats;

namespace Awv.Games.WoW.Stats
{
    /// <summary>
    /// A WoW <see cref="IStat"/> interface that just adds a <see cref="StatType"/>
    /// </summary>
    public interface IWoWStat : IStat
    {
        public StatType GetStatType();
    }
}
