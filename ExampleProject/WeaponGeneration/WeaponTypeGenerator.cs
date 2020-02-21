using Awv.Automation.Generation;
using Awv.Automation.Generation.Interface;
using Awv.Games.WoW.Items.Equipment;
using System.Collections.Generic;
using System.Linq;

namespace ExampleProject.WeaponGeneration
{
    public class WeaponTypeGenerator : IGenerator<EquipmentType>
    {
        private QueriedGenerator<IEnumerable<EquipmentType>, EquipmentType> TypeGenerator { get; set; }

        public WeaponTypeGenerator(IEnumerable<EquipmentTypeDefinition> definitions)
        {
            TypeGenerator = definitions
                .Where(def => def.Type == "Weapon")
                .SelectMany(def => def.Slots
                    .Select(slot => new EquipmentType(def, slot))
                ).AsGenerator();
        }

        public EquipmentType Generate(IRNG random)
            => TypeGenerator.Generate(random);
    }
}
