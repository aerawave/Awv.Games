using System;

namespace Awv.Games.WoW.Data.Data
{
    public enum ExportFormat
    {
        PlainText,
        Json
    }

    public static class ExportFormatExtensions
    {
        public static string GetExtension(this ExportFormat format)
        {
            switch (format)
            {
                case ExportFormat.PlainText: return "txt";
                case ExportFormat.Json: return "json";
                default: throw new ArgumentException($"{format} is not a valid generate format.", nameof(format));
            }
        }
    }
}
