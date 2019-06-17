using System;

namespace Pandora.Interactions.UI.Design.Converter
{
    class DefaultConverter : ConverterBase
    {
        private Type _type;

        public DefaultConverter(Type type)
        {
            _type = type;
        }

        public override object ConvertFromString(string value)
        {
            return Convert.ChangeType(value, _type);
        }

        public override string ConvertToString(object value)
        {
            return value.ToString();
        }
    }
}
