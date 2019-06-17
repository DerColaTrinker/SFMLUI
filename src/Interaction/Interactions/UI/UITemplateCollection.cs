using Pandora.Interactions.UI.Renderer;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Pandora.Interactions.UI
{
    public sealed class UITemplateCollection : IEnumerable<UIElement>
    {
        private List<UIElement> _elements = new List<UIElement>();
        private UIElement _parent;

        internal UITemplateCollection(UIElement parent)
        {
            _parent = parent;
        }

        public UIElement this[int index] { get => _elements[index]; set => _elements[index] = value; }

        internal void InternalRenderUpdate(RenderTargetBase target)
        {
            _elements.ForEach(m => m.InternalRenderUpdate(target));
        }

        internal void InternalPositionUpdate()
        {
            _elements.ForEach(m => m.UpdatePosition());
        }

        internal void InternalOnLoad(SceneHandler handler)
        {
            _elements.ForEach(m => m.InternalOnLoad(handler));
        }

        public int Count => _elements.Count;

        public void Add(UIElement item)
        {
            _elements.Add(item);
            item.InternalSetParent(_parent);
            item.Handler = _parent.Handler;
        }

        public void AddRange(IEnumerable<UIElement> elements)
        {
            foreach (var item in elements)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            _elements.Clear();
        }

        public bool Contains(UIElement item)
        {
            return _elements.Contains(item);
        }

        public IEnumerator<UIElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        public bool Remove(UIElement item)
        {
            return _elements.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
