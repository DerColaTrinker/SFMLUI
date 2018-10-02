using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace System.Xml
{
    public static class XmlAttributeCollectionExtentions
    {
        public static string GetValue(this XmlAttributeCollection collection, string name)
        {
            try
            {
                return collection[name].Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetValue(this XmlAttributeCollection collection, string name, string defaultvalue)
        {
            try
            {
                return collection[name].Value;
            }
            catch (Exception)
            {
                return defaultvalue;
            }
        }

        public static T GetValue<T>(this XmlAttributeCollection collection, string name, T defaultvalue)
        {
            try
            {
                var value = GetValue(collection, name);

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return defaultvalue;
            }
        }

        public static T GetEnum<T>(this XmlAttributeCollection collection, string name, T defaultvalue)
            where T : struct
        {
            try
            {
                var value = GetValue(collection, name);

                if (typeof(T).IsEnum)
                {
                    if (Enum.TryParse(value, true, out T result))
                    {
                        return result;
                    }
                }

                throw new InvalidCastException("Not an enum");
            }
            catch (Exception)
            {
                return defaultvalue;
            }
        }

    }
}
