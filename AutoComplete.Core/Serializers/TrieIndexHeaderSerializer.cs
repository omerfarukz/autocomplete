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
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
            {
                string propertySeperator = string.Empty;
                foreach (var property in properties)
                {
                    streamWriter.Write(propertySeperator);
                    streamWriter.Write(property.Name);
                    streamWriter.Write(KeyValueSeperator);

                    var propertyValue = property.GetValue(header);
                    if (propertyValue != null)
                    {
                        if (typeof(List<char>).GetTypeInfo().IsAssignableFrom(property.PropertyType.GetTypeInfo()))
                        {
                            var list = propertyValue as List<char>;
                            char itemSeperator = ' ';
                            foreach (var item in list)
                            {
                                streamWriter.Write(itemSeperator);
                                streamWriter.Write((int)item);
                                itemSeperator = TrieIndexHeaderSerializer.ItemSeperator;
                            }
                        }
                        else
                        {
                            streamWriter.Write(property.GetValue(header)?.ToString());
                        }
                    }
                    streamWriter.Write(Environment.NewLine);
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
                    string valueAsString = keyValue[1];

                    var property = properties.SingleOrDefault(f => f.Name == key);
                    if (property == null)
                    {
                        throw new Exception("Property not found");
                    }

                    object propertyValue = null;
                    if (typeof(List<char>).GetTypeInfo().IsAssignableFrom(property.PropertyType.GetTypeInfo()))
                    {
                        propertyValue = valueAsString.Split(ItemSeperator)
                                                     .Select(f => Convert.ToChar(int.Parse(f)))
                                                     .ToList();
                    }
                    else
                    {
                        propertyValue = Convert.ChangeType(valueAsString, property.PropertyType);
                    }

                    property.SetValue(header, propertyValue);
                }
            }

            return header;
        }

        private IEnumerable<PropertyInfo> GetProperties(TrieIndexHeader header)
        {
            return header.GetType().GetRuntimeProperties();
        }
    }
}
