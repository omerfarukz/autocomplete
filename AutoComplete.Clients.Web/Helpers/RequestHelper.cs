namespace AutoComplete.Clients.Web.Helpers
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.ComponentModel;

    public static class RequestHelper
    {
        public static T ExtractValue<T>(this HttpRequest request, string key, T defaultValue, RequestCollectionType requestCollectionType = RequestCollectionType.Query)
        {
            string stringValue = ExtractString(request, requestCollectionType, key);
            T value = defaultValue;

            if (stringValue != null)
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
                if (typeConverter != null)
                {
                    if (typeConverter.CanConvertFrom(typeof(string)))
                    {
                        value = (T)typeConverter.ConvertTo(stringValue, typeof(T));
                    }
                }
            }

            return value;
        }

        public static string ExtractString(this HttpRequest request, RequestCollectionType requestCollectionType, string key)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(key);

            string value = null;
            if (requestCollectionType == RequestCollectionType.Form && request.Form != null && request.Form.ContainsKey(key))
            {
                value = request.Form[key];
            }
            else if (requestCollectionType == RequestCollectionType.Query && request.Query != null && request.Query.ContainsKey(key))
            {
                value = request.Query[key];
            }
            else if (requestCollectionType == RequestCollectionType.Cookie && request.Cookies != null && request.Cookies.ContainsKey(key))
            {
                value = request.Cookies[key];
            }

            return value;
        }

        public enum RequestCollectionType
        {
            Unknown = 0,
            Form = 1,
            Query = 2,
            Cookie = 3
        }
    }
}