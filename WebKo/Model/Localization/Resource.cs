using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WebKo.Model.General;
using WebKo.Settings;

namespace WebKo.Model.Localization
{
    public class Resource : Log
    {
        #region Properties
        
        public string Value { get; set; }

        public string CultureInfoName { get; set; }

        public CultureInfo CultureInfo
        {
            get
            {
                return CultureInfo.GetCultureInfo(CultureInfoName);
            }
        }

        #endregion

        #region Constructor

        public Resource SetCultureInfoName(string cultureInfoName)
        {
            CultureInfoName = cultureInfoName;

            return this;
        }

        #endregion
    }
}
