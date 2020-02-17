namespace Awv.Games.Stats
{
    /// <summary>
    /// A simple stat interface, used commonly in role playing games.
    /// </summary>
    public interface IStat
    {
        decimal GetAmount();
        string GetName();
        bool IsQuantifiable();
        string GetDisplayString();
    }
}
