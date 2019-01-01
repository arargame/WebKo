using System;
using System.Collections.Generic;
using System.Text;
using WebKo.Model.General;

namespace WebKo.Settings
{
    public static class StringExtension
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static T? ToEnum<T>(this string value,T? defaultValue=null) where T : struct
        {
            if (string.IsNullOrWhiteSpace(value) && defaultValue != null)
            {
                return defaultValue;
            }
            else
            {
                var enumTypeName = typeof(T).Name;

                Log.Create(new Log(enumTypeName, "ToEnum", string.Format("Cannot convert implicitly from {0} as string to {1}", value, enumTypeName), LogType.Warning));
            }

            T result;
            return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
        }

    }
}
