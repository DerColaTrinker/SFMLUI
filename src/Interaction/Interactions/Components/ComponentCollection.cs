using Pandora.Interactions.UI;
using System.Collections;
using System.Collections.Generic;

namespace Pandora.Interactions.Components
{
    public sealed class ComponentCollection : ICollection<ComponentBase>
    {
        private List<ComponentBase> _components = new List<ComponentBase>();
        private ControlElement _element;

        public ComponentCollection(ControlElement element)
        {
            _element = element;
            //_runtime = element.Handler.Service.Runtime;
        }

        public int Count => _components.Count;

        public bool IsReadOnly => false;

        public void Add(ComponentBase item)
        {
            _components.Add(item);
            _element.Handler.Service.Runtime.RuntimeSystemUpdate += item.OnSystemUpdate;
        }

        public void Clear()
        {
            _components.ForEach(m => _element.Handler.Service.Runtime.RuntimeSystemUpdate -= m.OnSystemUpdate);
            _components.Clear();
        }

        public bool Contains(ComponentBase item)
        {
            return _components.Contains(item);
        }

        public void CopyTo(ComponentBase[] array, int arrayindex)
        {
            _components.CopyTo(array, arrayindex);
        }

        public IEnumerator<ComponentBase> GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        public bool Remove(ComponentBase item)
        {
            var result = _components.Remove(item);

            if (result)
                _element.Handler.Service.Runtime.RuntimeSystemUpdate -= item.OnSystemUpdate;

            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _components.GetEnumerator();
        }
    }
}
