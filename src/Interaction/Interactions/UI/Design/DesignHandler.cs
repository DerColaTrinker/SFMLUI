using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Animations;
using Pandora.Interactions.UI.Design.Converter;
using Pandora.Interactions.UI.Drawing;

namespace Pandora.Interactions.UI.Design
{
    public class DesignHandler
    {
        private readonly Dictionary<string, string> _resources = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        private static Dictionary<Type, ControlContainer> _controls = new Dictionary<Type, ControlContainer>();

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

                if (!_controls.TryGetValue(control, out ControlContainer container))
                {
                    container = new ControlContainer(control);
                    _controls.Add(control, container);
                }

                container.Styles.AddRange(ParseXmlStyles(container, controlnode));
                container.Animations.AddRange(ParseXmlAnimation(container, controlnode));
            }
        }

        internal static void PerformDesign(ControlElement control)
        {
            if (_controls.TryGetValue(control.GetType(), out ControlContainer container))
            {
                ApplyDesign(control, container);
            }
        }

        private static void ApplyDesign(ControlElement control, ControlContainer container)
        {
            ApplyStyles(control, container);
            ApplyAnimation(control, container);
        }

        private static void ApplyAnimation(ControlElement control, ControlContainer container)
        {
            foreach (var animationcontainer in container.Animations)
            {
                var animation = default(Animation);

                if (animationcontainer.Styles.Count() == 1)
                {
                    // Create Animation Effect
                    var item = animationcontainer.Styles.First();
                    var binding = (BindingProperty)item.Property.GetValue(control);
                    animation = CreateAnimation(binding, item);

                }
                else if (animationcontainer.Styles.Count() > 1)
                {

                }
                else
                    continue;

                var effect = CreateAnimationEffect(animationcontainer, animation, control);

                control.Effects.Add(effect);
            }
        }

        private static void ApplyStyles(ControlElement control, ControlContainer container)
        {
            foreach (var item in container.Styles)
            {
                var binding = (BindingProperty)item.Property.GetValue(control);
                binding.Value = TypeConverter(binding.PropertyType).ConvertFromString(item.Value);
            }
        }

        private IEnumerable<AnimationContainer> ParseXmlAnimation(ControlContainer container, XmlNode controlnode)
        {
            foreach (XmlNode node in controlnode.SelectNodes("animation"))
            {
                var eventmode = node.Attributes.GetValue("event");
                var groupname = node.Attributes.GetValue("groupname", string.Empty);

                var animation = new AnimationContainer()
                {
                    Event = (DesignAnimationEvents)Enum.Parse(typeof(DesignAnimationEvents), eventmode, true),
                    Groupname = groupname
                };

                animation.Styles = ParseXmlStyles(container, node);

                yield return animation;
            }
        }

        private IEnumerable<StyleContainer> ParseXmlStyles(ControlContainer container, XmlNode basenode)
        {
            foreach (XmlNode stylenode in basenode.SelectNodes("style"))
            {
                foreach (XmlNode node in stylenode.SelectNodes("set"))
                {
                    var name = node.Attributes.GetValue("property");
                    var duration = node.Attributes.GetValue("duration", 0);
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
                                 select new StyleContainer() { Property = p, Duration = duration, Value = value }).FirstOrDefault();

                    if (style == null) throw new Exception($"Property '{name}' not found");

                    yield return style;
                }
            }
        }

        private void ParseXmlTemplates(ControlContainer container, XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {

            }
        }

        private static ConverterBase TypeConverter(Type type)
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

        private static Animation CreateAnimation(BindingProperty binding, StyleContainer style)
        {
            switch (binding.PropertyType.Name)
            {
                case "Color":
                    return new ColorAnimation((BindingProperty<Color>)binding, (Color)TypeConverter(typeof(Color)).ConvertFromString(style.Value), style.Duration);

                case "Vector2F":
                    return new VectorAnimation((BindingProperty<Vector2F>)binding, (Vector2F)TypeConverter(typeof(Vector2F)).ConvertFromString(style.Value), style.Duration);

                default:
                    throw new Exception($"Animation converter not found '{binding.PropertyType.Name}'");
            }
        }

        private static AnimationEventHook CreateAnimationEffect(AnimationContainer container, Animation animation, ControlElement control)
        {
            var hook = new AnimationEventHook
            {
                Animation = animation,
                Control = control,
                Groupname = container.Groupname
            };

            switch (container.Event)
            {
                case DesignAnimationEvents.MouseEnter:
                    control.MouseEnter += hook.MouseEvents1;
                    break;

                case DesignAnimationEvents.MouseLeave:
                    control.MouseLeave += hook.MouseEvents1;
                    break;

                case DesignAnimationEvents.MouseClick:
                    control.MouseClick += hook.MouseEvents1;
                    break;

                default:
                    throw new NotSupportedException($"'{container.Event}' not supported.");
            }

            return hook;
        }
    }
}