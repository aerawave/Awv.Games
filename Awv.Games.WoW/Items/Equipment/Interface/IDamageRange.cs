namespace Awv.Games.WoW.Items.Equipment.Interface
{
    public interface IDamageRange
    {
        decimal GetMinimum();
        decimal GetMaximum();
        string GetElement();
        string GetDisplayString();
    }
}
