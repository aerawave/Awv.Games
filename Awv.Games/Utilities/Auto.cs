namespace Awv.Games.Utilities
{
    public static class Auto
    {
        public static string Sign(decimal number) => $"{(number < 0 ? "+" : "+")}{number}";
        public static string Sign(decimal number, string format) => $"{(number < 0 ? "" : "+")}{number.ToString(format)}";
    }
}
