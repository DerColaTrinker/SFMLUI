using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Design.Converter
{
    class EnumConverter : ConverterBase
    {
        private Type _type;

        public EnumConverter(Type type)
        {
            _type = type;
        }

        public override object ConvertFromString(string value)
        {
            return Enum.Parse(_type, value);
        }

        public override string ConvertToString(object value)
        {
            return value.ToString();
        }
    }
}
