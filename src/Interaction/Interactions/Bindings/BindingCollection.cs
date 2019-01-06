using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.Bindings
{
    public sealed class BindingCollection : DynamicObject
    {
        private Dictionary<string, BindingProperty> _bindings = new Dictionary<string, BindingProperty>();
        private BindingObject _parent;

        public BindingCollection(BindingObject parent)
        {
            _parent = parent;
        }

        public dynamic Accessor { get => this; }

        public BindingProperty<T> Create<T>(string name)
        {
            var binding = new BindingProperty<T>(name);

            _bindings.Add(name, binding);

            return binding;
        }

        public BindingProperty<T> Create<T>(string name, T value)
        {
            var binding = new BindingProperty<T>(name, value);

            _bindings.Add(name, binding);

            return binding;
        }

        public BindingProperty<T> Create<T>(string name, BindingGetter<T> getter, BindingSetter<T> setter)
        {
            var binding = new BindingProperty<T>(name, getter, setter);

            _bindings.Add(name, binding);

            return binding;
        }

        public BindingProperty<T> Create<T>(string name, BindingGetter<T> getter, BindingSetter<T> setter, T value)
        {
            var binding = new BindingProperty<T>(name, getter, setter, value);

            _bindings.Add(name, binding);

            return binding;
        }

        public BindingProperty<T> Create<T>(string name, BindingProperty<T> source)
        {
            var binding = new BindingProperty<T>(name, source);

            _bindings.Add(name, binding);

            return binding;
        }

        public BindingProperty CreateVirtual(string name, BindingProperty templatebinding)
        {
            var binding = new BindingProperty(name, templatebinding.PropertyType, templatebinding);

            _bindings.Add(name, binding);

            return binding;
        }

        public bool TryGetBinding<T>(string name, out BindingProperty<T> binding)
        {
            if (TryGetBinding(name, out BindingProperty basebinding))
            {
                binding = (BindingProperty<T>)basebinding;
                return true;
            }
            else
            {
                binding = null;
                return false;
            }
        }

        public bool TryGetBinding(string name, out BindingProperty binding)
        {
            return _bindings.TryGetValue(name, out binding);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!TryGetBinding(binder.Name, out BindingProperty property))
                return false;

            property.Value = value;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            return base.TryInvokeMember(binder, args, out result);
        }
    }
}
