using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.Bindings
{
    public sealed class BindingCollection
    {
        private Dictionary<string, BindingProperty> _bindings = new Dictionary<string, BindingProperty>();

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
    }
}
