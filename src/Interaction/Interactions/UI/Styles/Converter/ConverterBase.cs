using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Styles.Converter
{
    public abstract class ConverterBase
    {
        public virtual object ConvertFromString(string value)
        {
            return value;
        }

        public virtual string ConvertToString(object value)
        {
            return value.ToString();
        }
    }
}
