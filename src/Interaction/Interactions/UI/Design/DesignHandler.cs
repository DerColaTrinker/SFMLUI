using Pandora.Interactions.Bindings;
using Pandora.Interactions.Controller;
using Pandora.Interactions.UI.Animations;
using Pandora.Interactions.UI.Design.Converter;
using Pandora.Interactions.UI.Drawing;
using Pandora.Runtime.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Pandora.Interactions.UI.Design
{
    public class DesignHandler
    {
        private readonly Dictionary<string, Ressource> _resources = new Dictionary<string, Ressource>(StringComparer.InvariantCultureIgnoreCase);
        private static readonly Dictionary<Type, DesignContainer> _controls = new Dictionary<Type, DesignContainer>();

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

            foreach (XmlNode resnode in node.SelectNodes("resource"))
            {
                var name = resnode.Attributes.GetValue("name");
                var value = resnode.Attributes.GetValue("value");

                _resources[name] = new Ressource() { Value = value, ResourceType = ResourceType.Template };
            }
        }

        #region Style

        private void ParseStylesControls(XmlNode stylenode)
        {
            foreach (XmlNode controlnode in stylenode.SelectNodes("control"))
            {
                var controlname = controlnode.Attributes.GetValue("name");

                var control = (from a in AppDomain.CurrentDomain.GetAssemblies()
                               from rn in a.GetReferencedAssemblies()
                               let r = Assembly.Load(rn)
                               from t in r.GetTypes()
                               where t.IsClass && t.IsSubclassOf(typeof(ControlElement)) && t.FullName == controlname
                               select t).FirstOrDefault();
                if (control == null) throw new Exception($"Control '{controlname}' not found");

                if (!_controls.TryGetValue(control, out DesignContainer container))
                {
                    container = new DesignContainer(control);
                    _controls.Add(control, container);
                }

                container.Styles.AddRange(ParseXmlPropertySetter(controlnode));
                container.Animations.AddRange(ParseXmlAnimation(controlnode));
                container.Animations.AddRange(ParseXmlStoryboard(controlnode));
            }
        }

        private IEnumerable<PropertySetterContainer> ParseXmlPropertySetter(XmlNode basenode)
        {
            foreach (XmlNode node in basenode.SelectNodes("set"))
            {
                var name = node.Attributes.GetValue("property");
                var publicname = node.Attributes.GetValue("public", "");
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

        private IEnumerable<AnimationContainer> ParseXmlAnimation(XmlNode controlnode)
        {
            foreach (XmlNode node in controlnode.SelectNodes("animation"))
            {
                var eventmode = node.Attributes.GetValue("event");
                var groupname = node.Attributes.GetValue("groupname", string.Empty);

                var animation = new AnimationContainer()
                {
                    AnimationType = AnimationType.Normal,
                    Event = (TriggerEvents)Enum.Parse(typeof(TriggerEvents), eventmode, true),
                    Groupname = groupname
                };

                animation.PropertySetters = ParseXmlPropertySetter(node);

                yield return animation;
            }
        }

        private IEnumerable<AnimationContainer> ParseXmlStoryboard(XmlNode controlnode)
        {
            foreach (XmlNode node in controlnode.SelectNodes("storyboard"))
            {
                var eventmode = node.Attributes.GetValue("event");
                var groupname = node.Attributes.GetValue("groupname", string.Empty);

                var animation = new AnimationContainer()
                {
                    AnimationType = AnimationType.Storyboard,
                    Event = (TriggerEvents)Enum.Parse(typeof(TriggerEvents), eventmode, true),
                    Groupname = groupname
                };

                animation.PropertySetters = ParseXmlPropertySetter(node);

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

                if (!_controls.TryGetValue(basecontrol, out DesignContainer container))
                {
                    container = new DesignContainer(basecontrol);
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

                    var templatecontainer = new DesignContainer(uicontrol);
                    templatecontainer.Styles.AddRange(ParseXmlPropertySetter(controlnode));
                    container.Templates.Add(templatecontainer);
                }

                container.Animations.AddRange(ParseXmlAnimation(templatenode));
                container.Animations.AddRange(ParseXmlStoryboard(templatenode));
            }
        }

        #endregion

        internal static void ApplyDesignToControl(ControlElement control)
        {
            if (_controls.TryGetValue(control.GetType(), out DesignContainer container))
            {
                ApplyTemplates(control, container);
                ApplyStyles(control, container);
                ApplyAnimationTo(control, container);
            }
        }

        private static void ApplyTemplates(ControlElement control, DesignContainer container)
        {
            foreach (var template in container.Templates)
            {
                var templatecontrol = (UIElement)Activator.CreateInstance(template.Control);
                control.Templates.Add(templatecontrol);

                ApplyStyles(templatecontrol, template);
                ApplyPublicBindings(control, templatecontrol, template);
            }
        }

        private static void ApplyPublicBindings(ControlElement parentcontrol, UIElement templatecontrol, DesignContainer tempaltecontainer)
        {
            foreach (var item in tempaltecontainer.Styles)
            {
                if (!string.IsNullOrEmpty(item.PublicBindingName))
                {
                    if (!templatecontrol.Bindings.TryGetBinding(item.BindingName, out BindingProperty templatebinding))
                    {
                        //TODO: Die Property wurde nicht gefunden
                        continue;
                    }

                    parentcontrol.Bindings.CreateVirutal(item.PublicBindingName, templatebinding);
                }
            }
        }

        private static void ApplyAnimationTo(ControlElement control, DesignContainer container)
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
            var trigger = CreateTriggerAnimation(animationcontainer, animation, control);
            control.Triggers.Add(trigger);
        }

        private static void ApplyStyles(UIElement control, DesignContainer container)
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

        internal static Trigger CreateTriggerAnimation(AnimationContainer container, Animation animation, ControlElement control)
        {
            var hook = new Trigger
            {
                Animation = animation,
                Control = control,
                Groupname = container.Groupname
            };

            switch (container.Event)
            {
                case TriggerEvents.MouseEnter:
                    control.MouseEnter += hook.MouseEvents;
                    break;

                case TriggerEvents.MouseLeave:
                    control.MouseLeave += hook.MouseEvents;
                    break;

                case TriggerEvents.MouseClick:
                    control.MouseClick += hook.MouseEvents;
                    break;

                case TriggerEvents.MouseDown:
                    control.MouseButtonDown += hook.MouseButtonEvents;
                    break;

                case TriggerEvents.MouseUp:
                    control.MouseButtonUp += hook.MouseButtonEvents;
                    break;

                default:
                    throw new NotSupportedException($"'{container.Event}' not supported.");
            }

            return hook;
        }

        internal static Trigger CreateTrigger(ControlElement control, string groupname, TriggerEvents triggerevent, Animation animation)
        {
            var hook = new Trigger
            {
                Animation = animation,
                Control = control,
                Groupname = groupname
            };

            switch (triggerevent)
            {
                case TriggerEvents.MouseEnter:
                    control.MouseEnter += hook.MouseEvents;
                    break;

                case TriggerEvents.MouseLeave:
                    control.MouseLeave += hook.MouseEvents;
                    break;

                case TriggerEvents.MouseClick:
                    control.MouseClick += hook.MouseEvents;
                    break;

                case TriggerEvents.MouseDown:
                    control.MouseButtonDown += hook.MouseButtonEvents;
                    break;

                case TriggerEvents.MouseUp:
                    control.MouseButtonUp += hook.MouseButtonEvents;
                    break;

                default:
                    throw new NotSupportedException($"'{triggerevent}' not supported.");
            }

            return hook;
        }

        internal static Trigger CreateMouseButtonTrigger(ControlElement control, string groupname, TriggerEvents triggerevent, MouseButton button, Animation animation)
        {
            var hook = new MouseButtonTrigger()
            {
                Animation = animation,
                Control = control,
                Groupname = groupname,
                MouseButton = button
            };

            switch (triggerevent)
            {
                case TriggerEvents.MouseDown:
                    control.MouseButtonDown += hook.MouseButtonEvents;
                    break;

                case TriggerEvents.MouseUp:
                    control.MouseButtonUp += hook.MouseButtonEvents;
                    break;

                default:
                    throw new NotSupportedException($"'{triggerevent}' not supported.");
            }

            return hook;
        }
    }
}