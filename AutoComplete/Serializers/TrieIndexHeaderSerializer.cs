using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoComplete.DataStructure;

namespace AutoComplete.Serializers
{
    internal class TrieIndexHeaderSerializer
    {
        private const char KeyValueSeparator = ':';
        private const char ItemSeparator = ',';

        public void Serialize(Stream stream, TrieIndexHeader header)
        {
            var properties = GetProperties(header);
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            foreach (var property in properties)
            {
                writer.Write(property.Name);
                writer.Write(KeyValueSeparator);

                var propertyValue = property.GetValue(header);

                SerializePropertyValue(propertyValue, property.PropertyType, writer);

                writer.Write(Environment.NewLine);
            }
        }

        public TrieIndexHeader Deserialize(Stream stream)
        {
            var header = new TrieIndexHeader();
            var properties = GetProperties(header).ToArray();

            using var reader = new StreamReader(stream, Encoding.UTF8);
            while (reader.Peek() > -1)
            {
                var keyValuePair = reader.ReadLine()!.Split(KeyValueSeparator);
                var property = properties.Single(f => f.Name == keyValuePair[0]);
                var propertyValue = DeserializeValue(keyValuePair[1], property.PropertyType);
                property.SetValue(header, propertyValue);
            }

            return header;
        }

        private void SerializePropertyValue(object propertyValue, Type propertyType, StreamWriter writer)
        {
            if (propertyValue == null)
                return;
            
            if (typeof(List<char>).GetTypeInfo().IsAssignableFrom(propertyType.GetTypeInfo()))
            {
                var list = (List<char>) propertyValue;
                var itemSeparator = ' ';

                foreach (var item in list)
                {
                    writer.Write(itemSeparator);
                    writer.Write((int) item);
                    itemSeparator = ItemSeparator;
                }
            }
            else
            {
                writer.Write(propertyValue?.ToString());
            }
        }

        private object DeserializeValue(string valueAsString, Type propertyType)
        {
            object propertyValue = null;
            if (typeof(List<char>).GetTypeInfo().IsAssignableFrom(propertyType.GetTypeInfo()))
            {
                propertyValue = valueAsString.Split(ItemSeparator)
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