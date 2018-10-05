using Pandora.Interactions.UI.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Styles.Converter
{
    public sealed class ColorConverter : ConverterBase
    {
        public static ColorConverter Converter = new ColorConverter();

        public override object ConvertFromString(string value)
        {
            var parts = value.Split(',');
            var r = default(byte);
            var g = default(byte);
            var b = default(byte);
            var a = default(byte);

            switch (parts.Length)
            {
                case 1:
                    r = Convert.ToByte(parts[0].Trim());
                    a = 255;
                    break;

                case 2:
                    r = Convert.ToByte(parts[0].Trim());
                    g = Convert.ToByte(parts[1].Trim());
                    a = 255;
                    break;

                case 3:
                    r = Convert.ToByte(parts[0].Trim());
                    g = Convert.ToByte(parts[1].Trim());
                    b = Convert.ToByte(parts[2].Trim());
                    a = 255;
                    break;

                case 4:
                    r = Convert.ToByte(parts[0].Trim());
                    g = Convert.ToByte(parts[1].Trim());
                    b = Convert.ToByte(parts[2].Trim());
                    a = Convert.ToByte(parts[3].Trim());
                    break;

                default:
                    throw new InvalidCastException("Invalid color string " + value);
            }

            return new Color(r, g, b, a);
        }

        public override string ConvertToString(object value)
        {
            if (value is Color color)
                return string.Format("{0},{1},{2},{3}", color.R, color.G, color.B, color.A);

            throw new InvalidCastException("value ist not Color");
        }
    }
}
