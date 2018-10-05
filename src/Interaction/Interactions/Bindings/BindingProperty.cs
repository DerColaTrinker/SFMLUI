using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Styles;

namespace Pandora.Interactions.Bindings
{
    public delegate void BindingPropertyChangedDelegate(BindingProperty property, object value);
    public delegate void BindingPropertyChangedDelegate<T>(BindingProperty property, T value);

    public delegate T BindingGetter<T>();
    public delegate void BindingSetter<T>(T value);

    public class BindingProperty
    {
        private Delegate _getter;
        private Delegate _setter;

        public event BindingPropertyChangedDelegate BindingPropertyChanged;

        public BindingProperty(string name, Type type)
        {
            CallEvent = true;
            PropertyType = type;
            Name = name;

            BindingType = BindingPropertyType.InternalValue;
        }

        public BindingProperty(string name, Type type, object value) : this(name, type)
        {
            Value = value;
        }

        public BindingProperty(string name, Type type, Delegate getter, Delegate setter)
        {
            BindingType = BindingPropertyType.Delegate;
            CallEvent = true;
            PropertyType = type;
            Name = name;

            _getter = getter;
            _setter = setter;
        }

        public BindingProperty(string name, Type type, Delegate getter, Delegate setter, object value) : this(name, type, getter, setter)
        {
            Value = value;
        }

        public BindingProperty(string name, Type type, BindingProperty binding)
        {
            CallEvent = true;
            PropertyType = type;
            Name = name;

            _externalbinding = binding;
            BindingType = BindingPropertyType.ExternalBinding;
        }

        public bool CallEvent { get; set; }

        public Type PropertyType { get; protected set; }

        public string Name { get; protected set; }

        private BindingProperty _externalbinding;

        private object _internalvalue;

        public BindingPropertyType BindingType { get; }

        public object Value
        {
            get
            {
                switch (BindingType)
                {
                    case BindingPropertyType.ExternalBinding:
                        return _externalbinding.Value;

                    case BindingPropertyType.Delegate:

                        return _getter.DynamicInvoke();

                    case BindingPropertyType.InternalValue:
                        return _internalvalue;
                }

                return null;
            }
            set
            {
                switch (BindingType)
                {
                    case BindingPropertyType.ExternalBinding:
                        _externalbinding.Value = value;
                        break;

                    case BindingPropertyType.Delegate:
                        _setter.DynamicInvoke(new object[] { value });
                        break;

                    case BindingPropertyType.InternalValue:
                        _internalvalue = value;
                        break;
                }

                OnValueChanged(value);
            }
        }

        protected virtual void OnValueChanged(object value)
        {
            if (CallEvent) BindingPropertyChanged?.Invoke(this, value);
        }
    }

    public sealed class BindingProperty<T> : BindingProperty
    {
        public new event BindingPropertyChangedDelegate<T> BindingPropertyChanged;

        public BindingProperty(string name) : base(name, typeof(T))
        { }

        public BindingProperty(string name, T value) : base(name, typeof(T), value)
        { }

        public BindingProperty(string name, BindingGetter<T> getter, BindingSetter<T> setter) : base(name, typeof(T), getter, setter)
        { }

        public BindingProperty(string name, BindingGetter<T> getter, BindingSetter<T> setter, T value) : base(name, typeof(T), getter, setter, value)
        { }

        public BindingProperty(string name, BindingProperty<T> binding) : base(name, typeof(T), binding)
        { }

        public new T Value
        {
            get { return (T)base.Value; }
            set { base.Value = value; }
        }

        protected override void OnValueChanged(object value)
        {
            base.OnValueChanged(value);
            if (CallEvent) BindingPropertyChanged?.Invoke(this, (T)value);
        }
    }
}
