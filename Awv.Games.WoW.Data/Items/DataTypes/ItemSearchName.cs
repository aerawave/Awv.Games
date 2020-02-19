using System.Runtime.Serialization;

namespace Awv.Games.WoW.Data.Items.DataTypes
{
    /// <summary>
    /// See https://wow.tools/dbc/?dbc=itemsearchname
    /// </summary>
    public class ItemSearchName : IWoWData
    {
        [DataMember(Name="ID")]
        public int Id { get; set; }
        [DataMember(Name="Display_lang")]
        public string Name { get; set; }
        [DataMember(Name="OverallQualityID")]
        public int Rarity { get; set; }
        [DataMember(Name="ExpansionID")]
        public int ExpansionId { get; set; }

        public override string ToString() => Name;
    }
}
