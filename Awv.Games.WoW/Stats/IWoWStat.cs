using Awv.Games.Stats;

namespace Awv.Games.WoW.Stats
{
    public interface IWoWStat : IStat
    {
        public StatType GetStatType();
    }
}
