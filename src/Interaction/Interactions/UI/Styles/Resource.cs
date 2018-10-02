using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Styles
{
    public delegate void ResourceValueChangedDelegate(Resource sender);
    public delegate void ResourceValueChangedDelegate<T>(Resource<T> sender);

    public class Resource
    {
        private object _value;

        public event ResourceValueChangedDelegate ResourceValueChanged;

        public Resource(object value)
        {
            _value = value;
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; OnResourceValueChanged(); }
        }

        public Type Type { get { return _value.GetType(); } }

        protected virtual void OnResourceValueChanged()
        {
            ResourceValueChanged?.Invoke(this);
        }
    }

    public class Resource<T> : Resource
    {
        public new event ResourceValueChangedDelegate<T> ResourceValueChanged;

        public Resource(T value) : base(value)
        { }

        public new T Value
        {
            get { return (T)base.Value; }
            set { base.Value = value; }
        }

        public new Type Type { get { return typeof(T); } }

        protected override void OnResourceValueChanged()
        {
            base.OnResourceValueChanged();
            ResourceValueChanged?.Invoke(this);
        }
    }
}
