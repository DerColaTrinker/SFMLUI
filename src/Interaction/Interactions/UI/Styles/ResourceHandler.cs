using Pandora.Interactions.Bindings;
using System;
using System.Collections.Generic;

namespace Pandora.Interactions.UI.Styles
{
    public class ResourceHandler
    {
        private Dictionary<string, Resource> _resources = new Dictionary<string, Resource>(StringComparer.InvariantCultureIgnoreCase);

        internal ResourceHandler()
        { }

        public void Add(string key, Resource resource)
        {
            var tempres = (Resource)null;

            if (_resources.TryGetValue(key, out tempres))
            {
                if (resource.Type == tempres.Type)
                {
                    tempres.Value = resource.Value;
                }
                else
                    throw new InvalidCastException("Invalid resource type with existing resource");
            }
            else
                _resources[key] = resource;
        }

        public void Remove(string key)
        {
            _resources.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            return _resources.ContainsKey(key);
        }

        public Resource GetResource(string key)
        {
            var tempres = (Resource)null;

            _resources.TryGetValue(key, out tempres);

            return tempres;
        }

        public Resource<T> GetResource<T>(string key)
        {
            var tempres = (Resource)null;

            _resources.TryGetValue(key, out tempres);

            return (Resource<T>)tempres;
        }

        public bool IsType(string key, Type type)
        {
            var tempres = (Resource)null;

            if (_resources.TryGetValue(key, out tempres))
            {
                return tempres.Type == type;
            }

            return false;
        }

        public void BindToProperty(BindingProperty property, Resource resource)
        {
            property.Resource = resource;
        }
    }
}