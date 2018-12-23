using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;

namespace WebKo.Settings
{
    public class General
    {
        public static CultureInfo GetDefaultCultureInfo
        {
            get
            {
                return CultureInfo.GetCultureInfo(JsonConfigurationSection.GetValue("DefaultCultureName")) ?? CultureInfo.GetCultureInfo("en-us");
            }
        }
    }
}
