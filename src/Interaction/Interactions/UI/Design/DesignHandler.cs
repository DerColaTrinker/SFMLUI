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
        private readonly Dictionary<string, Ressource> _resources = new Dictionary<string, Ressource>(StringComparer.InvariantCultureIgnoreCase);
        private static Dictionary<Type, ControlContainer> _controls = new Dictionary<Type, ControlContainer>();

        public void Load(string filename)
        {
            var xd = new XmlDocument();
            xd.Load(filename);

            var stylesnode = xd.SelectSingleNode("styles");
            var templatenode = xd.SelectSingleNode("templates");

            ParseResources(stylesnode);
            ParseResources(templatenode);

            ParseStylesControls(xd.SelectNodes("styles/control"));
        }

        private void ParseResources(XmlNode node)
        {
            if (node == null) return;
            var mode = node.Name == "styles" ? ResourceType.Style : ResourceType.Template;

            foreach (XmlNode resnode in node.SelectNodes("resource"))
            {
                var name = resnode.Attributes.GetValue("name");
                var value = resnode.Attributes.GetValue("value");

                _resources[name] = new Ressource() { Value = value, ResourceType = ResourceType.Template };
            }
        }

        private void ParseStylesControls(XmlNodeList controlnodes)
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

                container.Styles.AddRange(ParseXmlPropertySetter(container, controlnode));
                container.Animations.AddRange(ParseXmlAnimation(container, controlnode));
                container.Animations.AddRange(ParseXmlStoryboard(container, controlnode));
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
                    AnimationType = AnimationType.Normal,
                    Event = (DesignAnimationEvents)Enum.Parse(typeof(DesignAnimationEvents), eventmode, true),
                    Groupname = groupname
                };

                animation.PropertySetters = ParseXmlPropertySetter(container, node);

                yield return animation;
            }
        }

        private IEnumerable<AnimationContainer> ParseXmlStoryboard(ControlContainer container, XmlNode controlnode)
        {
            foreach (XmlNode node in controlnode.SelectNodes("storyboard"))
            {
                var eventmode = node.Attributes.GetValue("event");
                var groupname = node.Attributes.GetValue("groupname", string.Empty);

                var animation = new AnimationContainer()
                {
                    AnimationType = AnimationType.Storyboard,
                    Event = (DesignAnimationEvents)Enum.Parse(typeof(DesignAnimationEvents), eventmode, true),
                    Groupname = groupname
                };

                animation.PropertySetters = ParseXmlPropertySetter(container, node);

                yield return animation;
            }
        }

        private IEnumerable<PropertySetterContainer> ParseXmlPropertySetter(ControlContainer container, XmlNode basenode)
        {
            foreach (XmlNode node in basenode.SelectNodes("set"))
            {
                var name = node.Attributes.GetValue("property");
                var duration = node.Attributes.GetValue("duration", 0);
                var start = node.Attributes.GetValue("start", 0);

                var value = node.InnerText;

                if (value.StartsWith("#"))
                {
                    if (_resources.TryGetValue(value.Substring(1), out Ressource ressource))
                    {
                        value = ressource.Value;
                    }
                    else
                    {
                        throw new Exception($"Resource '{value}' not found");
                    }
                }

                var style = (from p in container.Control.GetProperties()
                             where p.PropertyType.IsSubclassOf(typeof(BindingProperty)) && p.Name == name + "Binding"
                             select new PropertySetterContainer() { Property = p, Duration = duration, Start = start, Value = value }).FirstOrDefault();

                if (style == null) throw new Exception($"Property '{name}' not found");

                yield return style;
            }
        }

        private void ParseXmlTemplates(ControlContainer container, XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {

            }
        }

        internal static void PerformDesignTo(ControlElement control)
        {
            if (_controls.TryGetValue(control.GetType(), out ControlContainer container))
            {
                ApplyStyles(control, container);
                ApplyAnimationTo(control, container);
            }
        }

        private static void ApplyAnimationTo(ControlElement control, ControlContainer container)
        {
            foreach (var animationcontainer in container.Animations)
            {
                var animation = default(Animation);

                switch (animationcontainer.AnimationType)
                {
                    case AnimationType.Storyboard:
                        {
                            animation = new StoryboardAnimation();

                            foreach (var setter in animationcontainer.PropertySetters)
                            {
                                var item = animationcontainer.PropertySetters.First();
                                var binding = (BindingProperty)item.Property.GetValue(control);
                                ((StoryboardAnimation)animation).Add(setter.Start, CreateAnimation(binding, item));
                            }
                        }
                        break;

                    case AnimationType.Normal:
                        {
                            if (animationcontainer.PropertySetters.Count() == 1)
                            {
                                var item = animationcontainer.PropertySetters.First();
                                var binding = (BindingProperty)item.Property.GetValue(control);
                                animation = CreateAnimation(binding, item);
                            }
                            else if (animationcontainer.PropertySetters.Count() > 1)
                            {
                                animation = new GroupAnimation();

                                foreach (var setter in animationcontainer.PropertySetters)
                                {
                                    var item = animationcontainer.PropertySetters.First();
                                    var binding = (BindingProperty)item.Property.GetValue(control);
                                    ((GroupAnimation)animation).Add(CreateAnimation(binding, item));
                                }
                            }
                            else
                                continue;
                        }
                        break;

                    default:
                        throw new NotSupportedException(animationcontainer.AnimationType.ToString());
                }

                CreateAndAddAnimationEffect(control, animationcontainer, animation);
            }
        }

        private static void CreateAndAddAnimationEffect(ControlElement control, AnimationContainer animationcontainer, Animation animation)
        {
            var effect = CreateHookAnimation(animationcontainer, animation, control);
            control.Animations.Add(effect);
        }

        private static void ApplyStyles(ControlElement control, ControlContainer container)
        {
            foreach (var item in container.Styles)
            {
                var binding = (BindingProperty)item.Property.GetValue(control);
                binding.Value = TypeConverter(binding.PropertyType).ConvertFromString(item.Value);
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

        private static Animation CreateAnimation(BindingProperty binding, PropertySetterContainer style)
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

        private static AnimationEventHook CreateHookAnimation(AnimationContainer container, Animation animation, ControlElement control)
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