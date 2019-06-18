using System;

namespace Pandora.Interactions.UI.Design.Converter
{
    class EnumConverter : ConverterBase
    {
        private readonly Type _type;

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
