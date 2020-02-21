using Awv.Automation.Generation.Interface;
using Awv.Automation.Lexica;
using Awv.Games.WoW.Stats;
using System.Collections.Generic;

namespace ExampleProject.WeaponGeneration
{
    public class StatGenerator : IGenerator<IWoWStat>
    {
        public PhraseGenerator Phrases { get; set; }
        public decimal Amount { get; set; } = 0M;
        public StatType StatType { get; set; }
        
        public StatGenerator(StatType type, IEnumerable<Phrase> phrases)
        {
            StatType = type;
            Phrases = new PhraseGenerator(phrases);
        }

        public IWoWStat Generate(IRNG random)
            => new WoWStat(StatType, Phrases.Generate(random).ToString(), Amount);
    }
}
