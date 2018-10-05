using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Design.Converter;

namespace Pandora.Interactions.UI.Design
{
    public class DesignHandler
    {
        private readonly Dictionary<string, string> _resources = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        private static Dictionary<Type, DesignContainer> _controls = new Dictionary<Type, DesignContainer>();

        public void LoadDesign(string filename)
        {
            var xd = new XmlDocument();
            xd.Load(filename);

            ParseResources(xd);
            ParseXmlControls(xd.SelectNodes("controls/control"));
        }

        private void ParseResources(XmlDocument xd)
        {
            foreach (XmlNode resnode in xd.SelectNodes("controls/resource"))
            {
                var name = resnode.Attributes.GetValue("name");
                var value = resnode.Attributes.GetValue("value");

                _resources[name] = value;
            }
        }

        private void ParseXmlControls(XmlNodeList controlnodes)
        {
            foreach (XmlNode controlnode in controlnodes)
            {
                var controlname = controlnode.Attributes.GetValue("name");

                var control = (from a in AppDomain.CurrentDomain.GetAssemblies()
                               from t in a.GetTypes()
                               where t.IsClass && t.IsSubclassOf(typeof(ControlElement)) && t.FullName == controlname
                               select t).FirstOrDefault();
                if (control == null) throw new Exception($"Control '{controlname}' not found");

                if (!_controls.TryGetValue(control, out DesignContainer container))
                {
                    container = new DesignContainer(control);
                    _controls.Add(control, container);
                }

                ParseXmlTemplates(container, controlnode.SelectNodes("templates/element"));
                ParseXmlStyles(container, controlnode.SelectNodes("style/set"));
                ParseXmlAnimation(container, controlnode.SelectNodes("animation"));
            }
        }

        internal static void PerformDesign(ControlElement control)
        {
            if (_controls.TryGetValue(control.GetType(), out DesignContainer container))
            {
                ApplyStyles(control, container);
            }
        }

        private static void ApplyStyles(ControlElement control, DesignContainer container)
        {
            foreach (var item in container.Styles)
            {
                var binding = (BindingProperty)item.Property.GetValue(control);
                binding.Value = Converter(binding.PropertyType).ConvertFromString(item.Value);
            }
        }

        private void ParseXmlAnimation(DesignContainer container, XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {

            }
        }

        private void ParseXmlStyles(DesignContainer container, XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                var name = node.Attributes.GetValue("property");
                var value = node.InnerText;

                if (value.StartsWith("#"))
                {
                    if (_resources.TryGetValue(value.Substring(1), out string resvalue))
                    {
                        value = resvalue;
                    }
                    else
                    {
                        throw new Exception($"Resource '{value}' not found");
                    }
                }

                var style = (from p in container.Control.GetProperties()
                             where p.PropertyType.IsSubclassOf(typeof(BindingProperty)) && p.Name == name + "Binding"
                             select new StyleContainer() { Property = p, Value = value }).FirstOrDefault();

                if (style == null) throw new Exception($"Property '{name}' not found");

                container.Styles.Add(style);
            }
        }

        private void ParseXmlTemplates(DesignContainer container, XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {

            }
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
    }
}