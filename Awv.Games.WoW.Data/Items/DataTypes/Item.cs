using System.Runtime.Serialization;

namespace Awv.Games.WoW.Data.Items.DataTypes
{
    /// <summary>
    /// See https://wow.tools/dbc/?dbc=item
    /// </summary>
    public class Item : IWoWData
    {
        [DataMember(Name= "ID")]
        public int Id { get; set; }
        [DataMember(Name= "ClassID")]
        public int ClassId { get; set; }
        [DataMember(Name= "SubclassID")]
        public int SubClassId { get; set; }
        [DataMember(Name= "IconFileDataID")]
        public int IconFileId { get; set; }

        /// <summary>
        /// Post-loaded: <see cref="ListFile" />
        /// </summary>
        public string IconFilePath { get; set; }
        /// <summary>
        /// Post-loaded: <see cref="ListFile" />
        /// </summary>
        public string IconFileName { get; set; }

        /// <summary>
        /// Post-loaded: <see cref="ItemSearchName.Name" />
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Post-loaded: <see cref="ItemSearchName.Rarity" />
        /// </summary>
        public int Rarity { get; set; }
        /// <summary>
        /// Post-loaded: <see cref="ItemSearchName.ExpansionId" />
        /// </summary>
        public int ExpansionId { get; set; }

        /// <summary>
        /// Post-loaded: <see cref="ItemDatabase.ItemClasses"/>
        /// </summary>
        public ItemClass Class { get; set; }
        /// <summary>
        /// Post-loaded: <see cref="ItemDatabase.ItemSubClasses"/>
        /// </summary>
        public ItemSubClass SubClass { get; set; }

        public override string ToString() => Name;
    }
}
