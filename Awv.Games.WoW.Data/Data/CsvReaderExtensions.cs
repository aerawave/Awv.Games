using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Awv.Games.WoW.Data.Data
{
    public static class CsvReaderExtensions
    {
        public static void Bind(this CsvReader csv, object target)
        {
            var columns = csv.Context.NamedIndexes.Keys.ToArray();
            var type = target.GetType();
            var properties = type.GetProperties();

            foreach(var property in properties)
            {
                var members = property.GetCustomAttributes(typeof(DataMemberAttribute), true);
                foreach (DataMemberAttribute member in members)
                {
                    var name = member.Name;
                    if (!string.IsNullOrWhiteSpace(name) && columns.Contains(name))
                    {
                        var index = Array.IndexOf(columns, name);
                        if (property.PropertyType == typeof(int))
                        {
                            property.SetValue(target, csv.GetField<int>(index));
                        } else if (property.PropertyType.IsEnum) {
                            property.SetValue(target, Enum.ToObject(property.PropertyType, csv.GetField<int>(index)));
                        } else
                        {
                            property.SetValue(target, csv.GetField(index));
                        }
                    }
                }
            }
        }

        public static TDataType Read<TDataType>(this CsvReader csv)
            where TDataType : IWoWData
        {
            var data = Activator.CreateInstance<TDataType>();
            Bind(csv, data);
            return data;
        }

        public static IEnumerable<TDataType> ReadAll<TDataType>(this CsvReader csv)
            where TDataType : IWoWData
        {
            while (csv.Read())
            {
                yield return csv.Read<TDataType>();
            }
        }
    }
}
