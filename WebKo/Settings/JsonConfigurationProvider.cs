using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WebKo.Model.General;

namespace WebKo.Settings
{
    public class JsonConfigurationProvider
    {
        static List<JsonConfigurationProvider> List = new List<JsonConfigurationProvider>();

        public string Key { get; set; }

        public string Value { get; set; }

        public JsonConfigurationProvider(string key,string value)
        {
            Key = key;
            Value = value;
        }

        public static void SetValue(string key,string value)
        {
            JsonConfigurationProvider provider = new JsonConfigurationProvider(key, value);

            if (!List.Any(l => l.Key == provider.Key))
                List.Add(provider);
        }

        public static string GetValue(string key)
        {
            var value = "";

            try
            {
                value = List.SingleOrDefault(p => p.Key == key).Value;
            }
            catch (Exception ex)
            {
                Log.Create(ex.Message);
            }

            return value;
        }

        public static List<JsonConfigurationProvider> GetList
        {
            get
            {
                return List;
            }
        }
    }
}
