namespace Awv.Games.WoW.Items.Equipment.Interface
{
    public interface IDamageRange
    {
        decimal GetMinimum();
        decimal GetMaximum();
        /// <summary>
        /// The element of the inflicted damage.
        /// </summary>
        string GetElement();
        /// <summary>
        /// The preferred way of rendering this damage range.
        /// </summary>
        string GetDisplayString();
    }
}
