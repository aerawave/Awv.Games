using Awv.Games.Utilities;

namespace Awv.Games.WoW.Stats
{
    public class WoWStat : IWoWStat
    {
        private decimal amount;
        public string Name { get; set; }
        public StatType Type { get; set; }
        public decimal Amount
        {
            get => amount;
            set
            {
                amount = value;
                Quantifiable = true;
            }
        }
        public bool Quantifiable { get; set; } = false;

        public WoWStat(StatType type, string name)
        {
            Type = type;
            Name = name;
        }

        public WoWStat(StatType type, string name, decimal amount)
            : this(type, name)
        {
            Amount = amount;
        }

        public string GetName() => Name;

        public StatType GetStatType() => Type;

        public decimal GetAmount() => Amount;

        public bool IsQuantifiable() => Quantifiable;

        public string GetDisplayString() => IsQuantifiable() ? $"{Amount.ToString("+#;-#")} {Name}" : Name;

        public override string ToString() => GetDisplayString();
    }
}
