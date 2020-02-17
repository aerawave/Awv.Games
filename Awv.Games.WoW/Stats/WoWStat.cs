namespace Awv.Games.WoW.Stats
{
    public class WoWStat : IWoWStat
    {
        #region Properties
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
        #endregion
        #region IStat Accessors
        public string GetName() => Name;
        public decimal GetAmount() => Amount;
        public bool IsQuantifiable() => Quantifiable;
        public string GetDisplayString() => IsQuantifiable() ? $"{Amount.ToString("+#;-#")} {Name}" : Name;
        #endregion
        #region IWoWStat Accessors
        public StatType GetStatType() => Type;
        #endregion

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


        public override string ToString() => GetDisplayString();
    }
}
