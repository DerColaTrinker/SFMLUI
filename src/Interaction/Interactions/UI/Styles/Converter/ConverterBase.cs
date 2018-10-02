using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Styles.Converter
{
    public abstract class ConverterBase
    {
        public abstract StyleResourceType ConverterType { get; }

        public virtual object ConvertFromString(string value)
        {
            return value;
        }

        public virtual string ConvertToString(object value)
        {
            return value.ToString();
        }

        public virtual Type Type { get { return typeof(object); } }
    }
}
