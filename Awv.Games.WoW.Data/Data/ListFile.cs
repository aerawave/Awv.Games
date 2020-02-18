using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Awv.Games.WoW.Data
{
    /// <summary>
    /// See https://wow.tools/files
    /// </summary>
    public class ListFile : Dictionary<int,string>
    {
        public void Load(string filepath)
        {
            using var fs = File.Open(filepath, FileMode.Open);
            Load(fs);
        }

        public void Load(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);

            csv.Configuration.HasHeaderRecord = false;
            csv.Configuration.Delimiter = ";";
            while (csv.Read())
            {
                Add(int.Parse(csv.GetField(0)), csv.GetField(1));
            }
        }
    }
}
