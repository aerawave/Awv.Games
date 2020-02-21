using Awv.Automation.Generation;
using Awv.Automation.Generation.Interface;
using Awv.Lexica.Compositional.Interface;

namespace ExampleProject.WeaponGeneration
{
    public class Core : ILibrary
    {
        public static IRNG RNG { get; set; }
        public static WeaponGenerator Generator { get; set; }
        public static RangeGenerator Range { get; set; } = new RangeGenerator();

        public static string tag(string tagName)
        {
            return Generator.Phrases.Tagged(tagName).Generate(RNG).ToString();
        }

        public static string stat()
        {
            var which = ri(0, 2);

            switch (which)
            {
                case 1: return primarystat().ToString();
                case 2: return secondarystat().ToString();
                case 3: return tertiarystat().ToString();
            }
            return "";
        }

        public static int primarystat() => Generator.PrimaryStatCount.Generate(RNG);
        public static int secondarystat() => Generator.SecondaryStatCount.Generate(RNG);
        public static int tertiarystat() => Generator.TertiaryStatCount.Generate(RNG);

        public static int ri(int min, int max) => randomi(min, max);

        public static int randomi(int min, int max)
        {
            return Range.SetRange(min, max).Generate(RNG);
        }

        public static bool calculate_chance(double chance)
            => RNG.NextDouble() < chance;
    }
}
