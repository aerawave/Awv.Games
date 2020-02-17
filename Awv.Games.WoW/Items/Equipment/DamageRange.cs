using Awv.Games.WoW.Items.Equipment.Interface;
using System.Linq;

namespace Awv.Games.WoW.Items.Equipment
{
    /// <summary>
    /// A standard, basic damage range.
    /// </summary>
    public class DamageRange : IDamageRange
    {
        public decimal Minimum { get; set; }
        public decimal Maximum { get; set; }
        public string Element { get; set; }

        public DamageRange() { }

        public DamageRange(decimal min, decimal max) : this()
        {
            Minimum = min;
            Maximum = max;
        }

        public DamageRange(decimal min, decimal max, string element) : this(min, max)
        {
            Element = element;
        }

        public decimal GetMinimum() => Minimum;
        public decimal GetMaximum() => Maximum;
        public string GetElement() => Element;
        public string GetDisplayString() => string.Join(" ", new string[] {
            $"{Minimum} - {Maximum}",
            !string.IsNullOrWhiteSpace(Element) ? Element : null,
            "Damage"
        }.Where(str => str != null));

        public override string ToString() => GetDisplayString();
    }
}
