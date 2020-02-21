using System;

namespace Awv.Games.WoW.Data.Artwork
{
    public class ExceptionData
    {
        public uint Index { get; set; }
        public string File { get; set; }
        public string ExceptionMessage { get; set; }

        public ExceptionData() { }

        public ExceptionData(uint index, string file, string exceptionMessage)
            : this()
        {
            Index = index;
            File = file;
            ExceptionMessage = exceptionMessage;
        }

        public ExceptionData(uint index, string file, Exception exception)
            : this(index, file, exception.Message) { }
    }
}
