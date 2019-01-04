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

            var stylenode = xd.SelectSingleNode("styles");
            var templatesnode = xd.SelectSingleNode("templates");

            ParseResources(stylenode);
            ParseResources(templatesnode);

            if (stylenode != null) ParseStylesControls(stylenode);
            if (templatesnode != null) ParseTemplateControls(templatesnode);
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

        private void ParseTemplates(XmlNode templatesnode)
        {
            foreach (XmlNode teamplatenode in templatesnode.SelectNodes("template"))
            {
                var controlname = teamplatenode.Attributes.GetValue("name");
            }
        }

        #region Style

        private void ParseStylesControls(XmlNode stylenode)
        {
            foreach (XmlNode controlnode in stylenode.SelectNodes("control"))
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

        private IEnumerable<PropertySetterContainer> ParseXmlPropertySetter(ControlContainer container, XmlNode basenode)
        {
            foreach (XmlNode node in basenode.SelectNodes("set"))
            {
                var name = node.Attributes.GetValue("property");
                var publicname = node.Attributes.GetValue("public");
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

                yield return new PropertySetterContainer() { BindingName = name, PublicBindingName = publicname, Duration = duration, Start = start, Value = value };
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

        #endregion

        #region Templates

        private void ParseTemplateControls(XmlNode templatebasenode)
        {
            // Process all controls
            foreach (XmlNode templatenode in templatebasenode.SelectNodes("template"))
            {
                var typename = templatenode.Attributes.GetValue("type");

                var basecontrol = (from a in AppDomain.CurrentDomain.GetAssemblies()
                                   from t in a.GetTypes()
                                   where t.IsClass && t.IsSubclassOf(typeof(ControlElement)) && t.FullName == typename
                                   select t).FirstOrDefault();
                if (basecontrol == null) throw new Exception($"Base-Control '{typename}' not found");

                if (!_controls.TryGetValue(basecontrol, out ControlContainer container))
                {
                    container = new ControlContainer(basecontrol);
                    _controls.Add(basecontrol, container);
                }

                foreach (XmlNode controlnode in templatenode.SelectNodes("control"))
                {
                    var uicontrolname = controlnode.Attributes.GetValue("name");
                    var uicontrol = (from a in AppDomain.CurrentDomain.GetAssemblies()
                                     from t in a.GetTypes()
                                     where t.IsClass && t.IsSubclassOf(typeof(UIElement)) && t.Name == uicontrolname
                                     select t).FirstOrDefault();
                    if (uicontrol == null) throw new Exception($"Base-Control '{typename}' not found");

                    var templatecontainer = new ControlContainer(uicontrol);
                    templatecontainer.Styles.AddRange(ParseXmlPropertySetter(templatecontainer, controlnode));
                    container.Templates.Add(templatecontainer);
                }

                container.Animations.AddRange(ParseXmlAnimation(container, templatenode));
                container.Animations.AddRange(ParseXmlStoryboard(container, templatenode));
            }
        }

        #endregion

        internal static void ApplyDesignToControl(ControlElement control)
        {
            if (_controls.TryGetValue(control.GetType(), out ControlContainer container))
            {
                ApplyTemplates(control, container);
                ApplyStyles(control, container);
                ApplyAnimationTo(control, container);
            }
        }

        private static void ApplyTemplates(ControlElement control, ControlContainer container)
        {
            foreach (var template in container.Templates)
            {
                var templatecontrol = (UIElement)Activator.CreateInstance(template.Control);

                ApplyStyles(templatecontrol, template);
                ApplyPublicBindings(control, templatecontrol, template);

                control.Templates.Add(templatecontrol);
            }
        }

        private static void ApplyPublicBindings(ControlElement parentcontrol, UIElement templatecontrol, ControlContainer tempaltecontainer)
        {
            foreach (var item in tempaltecontainer.Styles)
            {
                if (!string.IsNullOrEmpty(item.PublicBindingName))
                {
                    if(!templatecontrol.Bindings.TryGetBinding(item.BindingName,out BindingProperty templatebinding))
                    {
                        //TODO: Die Property wurde nicht gefunden
                        continue;
                    }

                    parentcontrol.Bindings.Create(item.PublicBindingName, templatebinding);
                }
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
                                if (!control.Bindings.TryGetBinding(item.BindingName, out BindingProperty binding))
                                {
                                    //TODO: Error Binding not found.
                                    continue;
                                }

                                ((StoryboardAnimation)animation).Add(setter.Start, CreateAnimation(binding, item));
                            }
                        }
                        break;

                    case AnimationType.Normal:
                        {
                            if (animationcontainer.PropertySetters.Count() == 1)
                            {
                                var item = animationcontainer.PropertySetters.First();
                                if (!control.Bindings.TryGetBinding(item.BindingName, out BindingProperty binding))
                                {
                                    //TODO: Error Binding not found.
                                    continue;
                                }
                                animation = CreateAnimation(binding, item);
                            }
                            else if (animationcontainer.PropertySetters.Count() > 1)
                            {
                                animation = new GroupAnimation();

                                foreach (var setter in animationcontainer.PropertySetters)
                                {
                                    var item = animationcontainer.PropertySetters.First();
                                    if (!control.Bindings.TryGetBinding(item.BindingName, out BindingProperty binding))
                                    {
                                        //TODO: Error Binding not found.
                                        continue;
                                    }
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

        private static void ApplyStyles(UIElement control, ControlContainer container)
        {
            foreach (var item in container.Styles)
            {
                if (!control.Bindings.TryGetBinding(item.BindingName, out BindingProperty binding))
                {
                    //TODO: Error Binding not found.
                    continue;
                }

                binding.Value = TypeConverter(binding.PropertyType).ConvertFromString(item.Value);
            }
        }

        private static ConverterBase TypeConverter(Type type)
        {
            if (type.IsEnum)
            {
                return new EnumConverter(type);
            }
            else
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