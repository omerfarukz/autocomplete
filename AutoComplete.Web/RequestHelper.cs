namespace AutoComplete.Web
{
    using System;
    using System.ComponentModel;
    using System.Web;

    internal static class RequestHelper
    {
        private static HttpRequest _request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }

        public static T Get<T>(string key)
        {
            if (key == null)
                throw new ArgumentException("key");

            return Get<T>(_request, key, default(T));
        }

        public static T Get<T>(string key, T defaultValue)
        {
            if (key == null)
                throw new ArgumentException("key");

            return Get<T>(_request, key, defaultValue);
        }

        public static T Get<T>(this HttpRequest request, string key)
        {
            if (key == null)
                throw new ArgumentException("key");

            return Get<T>(key, default(T));
        }

        public static T Get<T>(this HttpRequest request, string key, T defaultValue)
        {
            if (key == null)
                throw new ArgumentException("key");

            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));

            #region Checks

            string objValue = null;

            if (request.QueryString[key] != null)
                objValue = _request.QueryString[key];
            else if (request.Form[key] != null)
                objValue = _request.Form[key];

            if (objValue == null)
                return defaultValue;

            if (typeConverter == null || !typeConverter.IsValid(objValue))
                return defaultValue;

            #endregion

            try
            {
                return (T)typeConverter.ConvertFrom(objValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
