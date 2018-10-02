using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Styles.Converter;

namespace Pandora.Interactions.UI.Styles
{
    public static class StyleHandler
    {
        private static Dictionary<string, Style> _styles = new Dictionary<string, Style>(StringComparer.InvariantCultureIgnoreCase);
        private static Dictionary<Type, StyleControlDescription> _controls = new Dictionary<Type, StyleControlDescription>();

        public static void LoadStyle(string filename)
        {
            if (!File.Exists(filename))
            {
                return;
            }

            var xd = new XmlDocument();

            try
            {
                xd.Load(filename);
            }
            catch (Exception)
            {
                return;
            }

            ParseXmlStyles(xd);
        }

        private static void ParseXmlStyles(XmlDocument xd)
        {
            foreach (XmlNode stylenode in xd.SelectNodes("styles/style"))
            {
                var stylename = stylenode.Attributes.GetValue("name", string.Empty);

                if (string.IsNullOrEmpty(stylename))
                {
                    continue;
                }

                var propertyvalues = new List<StylePropertyValueSetter>();
                foreach (XmlNode propertynode in stylenode.SelectNodes("property"))
                {
                    var name = propertynode.Attributes.GetValue("name", string.Empty);
                    var value = propertynode.InnerText;

                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    propertyvalues.Add(new StylePropertyValueSetter(name, value));
                }

                var style = new Style()
                {
                    Name = stylename,
                    Setter = propertyvalues.AsEnumerable()
                };

                _styles[stylename] = style;
            }
        }

        internal static Style GetStyle(string stylename)
        {
            if (!_styles.TryGetValue(stylename, out Style result))
            {
                //TODO: Fehler Style nicht gefunden
            }

            return result;
        }

        internal static void ApplyStyle(ControlElement instance, Style style)
        {
            var type = instance.GetType();

            // Control Binding Konfiguration erstellen wenn nicht verfügbar
            if (!_controls.TryGetValue(type, out StyleControlDescription controldescription))
            {
                var bindingproperties = from p in type.GetProperties()
                                        where p.PropertyType.IsSubclassOf(typeof(BindingProperty))
                                        let bp = (BindingProperty)p.GetValue(instance)
                                        select new StylePropertyDescription() { Property = p, Name = bp.Name, Type = bp.PropertyType, Binding = bp };

                controldescription = new StyleControlDescription()
                {
                    Type = type,
                    BindingProperties = bindingproperties.AsEnumerable()
                };

                _controls[type] = controldescription;
            }

            foreach (var setter in style.Setter)
            {
                var binding = controldescription.BindingProperties.Where(m => m.Name == setter.Name | m.Property.Name == setter.Name).FirstOrDefault();

                if (binding == null)
                {
                    //TODO: Fehler Binding nicht gefunden
                }

                if (Converter(binding, setter, out object value))
                {
                    binding.Binding.Value = value;
                }
            }
        }

        private static bool Converter(StylePropertyDescription binding, StylePropertyValueSetter setter, out object result)
        {
            result = null;

            switch (Type.GetTypeCode(binding.Type))
            {
                case TypeCode.Object:
                    var converter = default(ConverterBase);

                    switch (binding.Type.Name)
                    {
                        case "Color":
                            converter = new ColorConverter();
                            break;

                        case "Vector2":
                        case "Vector2F":
                        case "Vector2U":
                            converter = new VectorConverter();
                            break;

                        default:
                            return false;
                    }

                    result = converter.ConvertFromString(setter.Value);
                    return true;

                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    result = Convert.ChangeType(setter.Value, binding.Type);
                    return true;

                case TypeCode.Empty:
                case TypeCode.DBNull:
                default:
                    //TODO: Konvertierung ungültig.
                    return false;
            }
        }
    }
}
