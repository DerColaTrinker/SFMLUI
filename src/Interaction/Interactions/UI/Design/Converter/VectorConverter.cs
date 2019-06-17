using Pandora.Interactions.UI.Drawing;
using System;

namespace Pandora.Interactions.UI.Design.Converter
{
    public sealed class VectorConverter : ConverterBase
    {
        public static VectorConverter Converter = new VectorConverter();

        public override object ConvertFromString(string value)
        {
            var parts = value.Split(',');
            var x = default(float);
            var y = default(float);

            switch (parts.Length)
            {
                case 1:
                    x = y = Convert.ToSingle(parts[0].Trim());
                    break;

                case 2:
                    x = Convert.ToSingle(parts[0].Trim());
                    y = Convert.ToSingle(parts[1].Trim());
                    break;

                default:
                    throw new InvalidCastException("Invalid vector string " + value);
            }

            return new Vector2F(x, y);
        }

        public override string ConvertToString(object value)
        {
            if (value is Vector2F vector)
                return string.Format("{0}, {1}", vector.X, vector.Y);

            throw new InvalidCastException("value ist not Vector2F");
        }
    }
}
