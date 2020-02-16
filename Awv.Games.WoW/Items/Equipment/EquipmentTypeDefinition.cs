namespace Awv.Games.WoW.Items.Equipment
{
    public class EquipmentTypeDefinition
    {
        /// <summary>
        /// Ex: "One-Handed Axes" / "Two-Handed Swords" / "Daggers"
        /// </summary>
        public string VerboseName { get; set; }
        /// <summary>
        /// Ex: "Axe" / "Sword" / "Mace"
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ex: "Weapon" / "Armor" (used for shields)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Ex: "melee" / "ranged"
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// Ex: "One-Handed" / "Main Hand" / "Off-Hand" / "Ranged"
        /// </summary>
        public string[] Slots { get; set; }

        public EquipmentTypeDefinition(string verboseName, string name, string type, string range, string[] slots)
        {
            VerboseName = verboseName;
            Name = name;
            Type = type;
            Range = range;
            Slots = slots;
        }

        public override string ToString() => VerboseName;
    }
}
