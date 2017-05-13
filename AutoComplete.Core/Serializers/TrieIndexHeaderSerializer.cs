using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutoComplete.Core.Serializers
{
    internal class TrieIndexHeaderSerializer
    {
        const char KeyValueSeperator = ':';
        const char ItemSeperator = ',';

        public void Serialize(Stream stream, TrieIndexHeader header)
        {
            var properties = GetProperties(header);
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                foreach (var property in properties)
                {
                    writer.Write(property.Name);
                    writer.Write(KeyValueSeperator);

                    var propertyValue = property.GetValue(header);

                    SerializePropertyValue(propertyValue, property.PropertyType, writer);

                    writer.Write(Environment.NewLine);
                }
            }
        }

        public TrieIndexHeader Deserialize(Stream stream)
        {
            var header = new TrieIndexHeader();
            var properties = GetProperties(header);

            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                while (reader.Peek() > -1)
                {
                    string[] keyValue = reader.ReadLine().Split(KeyValueSeperator);
                    string key = keyValue[0];

                    var property = properties.SingleOrDefault(f => f.Name == key);
                    if (property == null)
                    {
                        throw new Exception("Property not found");
                    }

                    object propertyValue = DeserializeValue(keyValue[1], property.PropertyType);
                    property.SetValue(header, propertyValue);
                }
            }

            return header;
        }

        private void SerializePropertyValue(object propertyValue, Type propertyType, StreamWriter writer)
        {
            if (propertyValue != null)
            {
                if (typeof(List<char>).GetTypeInfo().IsAssignableFrom(propertyType.GetTypeInfo()))
                {
                    var list = (List<char>)propertyValue;
                    char itemSeperator = ' ';

                    foreach (var item in list)
                    {
                        writer.Write(itemSeperator);
                        writer.Write((int)item);
                        itemSeperator = TrieIndexHeaderSerializer.ItemSeperator;
                    }
                }
                else
                {
                    writer.Write(propertyValue?.ToString());
                }
            }
        }

        private object DeserializeValue(string valueAsString, Type propertyType)
        {
            object propertyValue = null;
            if (typeof(List<char>).GetTypeInfo().IsAssignableFrom(propertyType.GetTypeInfo()))
            {
                propertyValue = valueAsString.Split(ItemSeperator)
                                             .Select(f => Convert.ToChar(int.Parse(f)))
                                             .ToList();
            }
            else
            {
                propertyValue = Convert.ChangeType(valueAsString, propertyType);
            }

            return propertyValue;
        }

        private IEnumerable<PropertyInfo> GetProperties(TrieIndexHeader header)
        {
            return header.GetType().GetRuntimeProperties();
        }
    }
}
