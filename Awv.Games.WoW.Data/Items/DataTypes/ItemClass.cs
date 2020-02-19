using System.Runtime.Serialization;

namespace Awv.Games.WoW.Data.Items.DataTypes
{
    /// <summary>
    /// See https://wow.tools/dbc/?dbc=itemclass
    /// </summary>
    public class ItemClass : IWoWData
    {
        [DataMember(Name="ID")]
        public int Id { get; set; }
        [DataMember(Name= "ClassID")]
        public int ClassId { get; set; }
        [DataMember(Name= "ClassName_lang")]
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}
