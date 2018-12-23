using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WebKo.Model.General;

namespace WebKo.Settings
{
    public class JsonConfigurationSection
    {
        static List<JsonConfigurationSection> List = new List<JsonConfigurationSection>();

        public string Category { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public JsonConfigurationSection(string key,string value,string category = null)
        {
            Key = key;
            Value = value;
            Category = category;
        }

        public static void SetValue(string key,string value,string category = null)
        {
            JsonConfigurationSection provider = new JsonConfigurationSection(key, value,category);

            if (!List.Any(l => l.Key == provider.Key && l.Category == provider.Category))
                List.Add(provider);
        }

        public static string GetValue(string key,string category = null)
        {
            var value = "";

            try
            {
                value = List.SingleOrDefault(p => p.Key == key && p.Category == category).Value;
            }
            catch (Exception ex)
            {
                Log.Create(ex.Message);
            }

            return value;
        }

        public static List<JsonConfigurationSection> GetList
        {
            get
            {
                return List;
            }
        }
    }
}
