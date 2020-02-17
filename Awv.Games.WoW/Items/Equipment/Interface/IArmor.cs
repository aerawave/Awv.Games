namespace Awv.Games.WoW.Items.Equipment.Interface
{
    /// <summary>
    /// An interface for a piece of <see cref="IEquipment"/> which can be equipped in an armor slot.
    /// </summary>
    public interface IArmor : IEquipment
    {
        decimal GetArmorPoints();
    }
}
