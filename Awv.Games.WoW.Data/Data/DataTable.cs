using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Awv.Games.WoW.Data.Data
{
    public class DataTable<TDataType> : Dictionary<int, TDataType>
        where TDataType : IWoWData
    {
        public void Load(string filepath, Action<TDataType> predicate = null)
        {
            using var fs = File.Open(filepath, FileMode.Open);
            Load(fs, predicate);
        }

        public void Load(Stream stream, Action<TDataType> action = null)
        {
            Clear();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);

            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var data = csv.Read<TDataType>();
                action?.Invoke(data);
                Add(data.Id, data);
            }
        }
    }
}
