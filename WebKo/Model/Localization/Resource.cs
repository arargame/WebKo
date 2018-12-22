using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WebKo.Model.General;

namespace WebKo.Model.Localization
{
    public class Resource : Entity
    {
        public string Value { get; set; }

        public string CultureInfoName { get; set; }

        public CultureInfo CultureInfo
        {
            get
            {
                return CultureInfo.GetCultureInfo(CultureInfoName);
            }
        }
    }
}
