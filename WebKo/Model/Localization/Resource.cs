using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WebKo.Model.General;
using WebKo.Settings;

namespace WebKo.Model.Localization
{

    /// <summary>
    /// CultureInfoName = en-us,tr-tr 
    /// Value : Lorem ipsum
    /// Name : Class1
    /// PropertyName : Description
    /// Id = 124124
    /// Key-Value şeklinde olanlar ile id si şu olan şu tipten nesnenin alanının çeviriside sunulacak
    /// </summary>

    public class Resource : Log
    {
        #region Properties
        
        public string PropertyName { get; set; }

        public Guid? EntityId { get; set; }

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
