using System;
using System.Linq;

namespace Awv.Games.WoW.Items.Equipment
{
    public class EquipmentType
    {
        private string slot;
        public EquipmentTypeDefinition Definition { get; set; }
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
    }
}
