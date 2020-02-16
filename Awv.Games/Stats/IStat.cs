namespace Awv.Games.Stats
{
    public interface IStat
    {
        decimal GetAmount();
        string GetName();
        bool IsQuantifiable();
        string GetDisplayString();
    }
}
