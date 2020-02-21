using Awv.Automation.Generation.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleProject.WeaponGeneration
{
    public class CountGenerator : IGenerator<int>
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public Func<int, double> ChanceFunc { get; set; }

        public CountGenerator(int minimum, int maximum, Func<int, double> chanceFunc)
        {
            Minimum = minimum;
            Maximum = maximum;
            ChanceFunc = chanceFunc;
        }

        public CountGenerator(int minimum, int maximum, double chance)
            : this(minimum, maximum, num => chance) { }

        public int Generate(IRNG random)
        {
            var count = Math.Max(Minimum, 0);
            var continueRolling = true;

            while (continueRolling)
            {
                var chance = ChanceFunc(count);
                continueRolling = count < Maximum && random.NextDouble() < chance;
                if (continueRolling)
                    count++;
            }

            return count;
        }
    }
}
