using System.Runtime.Serialization;

namespace Awv.Games.WoW.Data.Data.Items.DataTypes
{
    /// <summary>
    /// See https://wow.tools/dbc/?dbc=itemsubclass
    /// </summary>
    public class ItemSubClass : IWoWData
    {
        [DataMember(Name="ID")]
        public int Id { get; set; }
        [DataMember(Name="ClassID")]
        public int ClassId { get; set; }
        [DataMember(Name="SubClassID")]
        public int SubClassId { get; set; }
        [DataMember(Name="DisplayName_lang")]
        public string Name { get; set; }
        [DataMember(Name= "VerboseName_lang")]
        public string VerboseName { get; set; }

        /// <summary>
        /// Post-loaded: <see cref="ItemDatabase.ItemClasses"/>
        /// </summary>
        public ItemClass Class { get; set; }

        public override string ToString() => VerboseName;
    }
}
