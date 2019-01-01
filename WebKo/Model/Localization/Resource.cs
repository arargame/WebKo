using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WebKo.Model.General;

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

    public class Resource : Entity
    {
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
    }
}
