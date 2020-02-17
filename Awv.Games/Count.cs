namespace Awv.Games
{
    /// <summary>
    /// A class for holding a count of a given object.
    /// </summary>
    /// <typeparam name="TCounted">The type being counted</typeparam>
    public class Count<TCounted>
    {
        /// <summary>
        /// The value being counted.
        /// </summary>
        public TCounted Value { get; set; }
        /// <summary>
        /// The count of a given <see cref="Value"/>.
        /// </summary>
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
