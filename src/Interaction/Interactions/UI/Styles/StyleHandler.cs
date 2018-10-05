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
        private static readonly Dictionary<string, string> _resources = new Dictionary<string, string>();
        private static Dictionary<Type, Dictionary<string, Style>> _controls = new Dictionary<Type, Dictionary<string, Style>>();

        public static void LoadStyle(string filename)
        {
            var xd = new XmlDocument();
            xd.Load(filename);

            ParseResources(xd);
            ParseXmlStyles(xd);
        }

        private static void ParseResources(XmlDocument xd)
        {
            foreach (XmlNode resnode in xd.SelectNodes("styleset/resource"))
            {
                var name = resnode.Attributes.GetValue("name");
                var value = resnode.Attributes.GetValue("value");

                _resources[name] = value;
            }
        }

        private static void ParseXmlStyles(XmlDocument xd)
        {
            foreach (XmlNode stylenode in xd.SelectNodes("styleset/style"))
            {
                var controlname = stylenode.Attributes.GetValue("control");
                var controltype = LookupControlElement(controlname);
                if (controltype == null) throw new Exception($"Control '{controlname}' not found");

                if (!_controls.TryGetValue(controltype, out Dictionary<string, Style> styles))
                {
                    styles = new Dictionary<string, Style>();
                    _controls.Add(controltype, styles);
                }

                ParseXmlStyleSetter(stylenode, controltype, styles);
            }
        }

        private static void ParseXmlStyleSetter(XmlNode stylenode, Type controltype, Dictionary<string, Style> styles)
        {
            foreach (XmlNode setnode in stylenode.SelectNodes("set"))
            {
                var name = setnode.Attributes.GetValue("property");
                var value = setnode.InnerText;

                if (value.StartsWith("#"))
                {
                    if (!_resources.TryGetValue(value.Substring(1), out string resvalue))
                        throw new Exception($"´Resource '{value}' not found");
                    else
                        value = resvalue;
                }

                var property = (from p in controltype.GetProperties()
                                where p.PropertyType.IsSubclassOf(typeof(BindingProperty))
                                where p.Name.Contains(name)
                                select p).FirstOrDefault();

                if (property == null) throw new Exception($"Property '{name}' not found");

                var style = new Style
                {
                    Property = property,
                    Value = value
                };

                styles[property.Name] = style;
            }
        }

        private static Type LookupControlElement(string controlname)
        {
            return (from a in AppDomain.CurrentDomain.GetAssemblies()
                    from t in a.GetTypes()
                    where t.IsClass && t.IsSubclassOf(typeof(ControlElement))
                    where t.FullName == controlname
                    select t).FirstOrDefault();
        }

        private static ConverterBase Converter(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Object:
                    switch (type.Name)
                    {
                        case "Color":
                            return ColorConverter.Converter;

                        case "Vector2":
                        case "Vector2F":
                        case "Vector2U":
                            return VectorConverter.Converter;

                        default:
                            throw new Exception($"Binding type '{type.Name}' not supported");
                    }

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
                    return new DefaultConverter(type);

                case TypeCode.Empty:
                case TypeCode.DBNull:
                default:
                    throw new Exception($"Binding type '{type.Name}' not supported");
            }
        }

        public static void ApplyStyle(ControlElement element)
        {
            if (_controls.TryGetValue(element.GetType(), out Dictionary<string, Style> styles))
            {
                foreach (var style in styles.Values)
                {
                    var binding = (BindingProperty)style.Property.GetValue(element);
                    binding.Value = Converter(binding.PropertyType).ConvertFromString(style.Value);
                }
            }
        }
    }
}