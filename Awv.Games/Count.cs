namespace Awv.Games
{
    public class Count<TCounted>
    {
        public TCounted Value { get; set; }
        public long Quantity { get; set; } = 0;

        public Count(TCounted value, long quantity)
        {
            Value = value;
            Quantity = quantity;
        }

        public Count(TCounted value)
            : this (value, 0)
        {
        }

        public override string ToString() => $"{Quantity} * {Value}";
    }
}
