using System;
using System.Linq;

namespace Awv.Games.WoW.Items.Equipment
{
    /// <summary>
    /// A type of equipment. This is used for determining what slot a piece of equipment can occupy, and what type of item it is in that slot.
    /// </summary>
    public class EquipmentType
    {
        private string slot;
        /// <summary>
        /// The definition of an equipment type.
        /// </summary>
        public EquipmentTypeDefinition Definition { get; set; }
        /// <summary>
        /// The slot of equipment the equipment would occupy.
        /// </summary>
        public string Slot
        {
            get => slot;
            set
            {
                if (!Definition.Slots.Contains(value))
                    throw new ArgumentOutOfRangeException(nameof(Slot));
                slot = value;
            }
        }

        public EquipmentType(EquipmentTypeDefinition definition, string slot)
        {
            Definition = definition;
            Slot = slot;
        }

        public EquipmentType(string slot, string name, string type)
            : this(new EquipmentTypeDefinition($"{slot} {name}", name, type, null, new string[] { slot }), slot)
        {

        }

        /// <summary>
        /// Shorthand for instantiating a weapon <see cref="EquipmentType"/>.
        /// </summary>
        public class Weapon : EquipmentType
        {
            /// <summary>
            /// Shorthand for instantiating a weapon <see cref="EquipmentType"/>.
            /// </summary>
            public Weapon(string slot, string name) : base(slot, name, "weapon") { }
        }

        /// <summary>
        /// Shorthand for instantiating an armor <see cref="EquipmentType"/>.
        /// </summary>
        public class Armor : EquipmentType
        {
            /// <summary>
            /// Shorthand for instantiating an armor <see cref="EquipmentType"/>.
            /// </summary>
            public Armor(string slot, string name) : base(slot, name, "armor") { }
        }
    }
}
