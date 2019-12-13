using Pandora.Interactions.Controller;
using Pandora.Interactions.UI.Design;
using Pandora.Interactions.UI.Renderer;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Pandora.Interactions.UI
{
    public sealed class ControlCollection : IEnumerable<ControlElement>
    {
        private readonly List<ControlElement> _elements = new List<ControlElement>();
        private readonly ControlElement _parent;

        internal ControlCollection(ControlElement parent)
        {
            Logger.Debug("[Interaction] Create ControlCollection instance : " + (parent == null ? "---" : parent.GetType().Name));
            _parent = parent;
        }

        public ControlElement this[int index] { get => _elements[index]; set => _elements[index] = value; }

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

        public void Add(ControlElement item)
        {
            _elements.Add(item);
            item.InternalSetParent(_parent);
            DesignHandler.ApplyDesignToControl(item);
            Logger.Trace($"[ControlCollection] Add control '{item.GetType().Name}'");
        }

        public void Insert(int index, ControlElement item)
        {
            _elements.Insert(index, item);
            item.InternalSetParent(_parent);
            item.UpdatePosition();
            DesignHandler.ApplyDesignToControl(item);
            Logger.Trace($"[ControlCollection] Add control '{item.GetType().Name}'");
        }

        public void Clear()
        {
            _elements.Clear();
            Logger.Trace($"[ControlCollection] Clear collection");
        }

        public bool Contains(ControlElement item)
        {
            return _elements.Contains(item);
        }

        public IEnumerator<ControlElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        public bool Remove(ControlElement item)
        {
            var result = _elements.Remove(item);

            if (result)
                Logger.Trace($"[ControlCollection] Remove control '{item.GetType().Name}'");

            return result;
        }

        internal bool RemoveAt(int index)
        {
            var item = _elements[index];

            if (item != null)
                Logger.Trace($"[ControlCollection] Remove control '{item.GetType().Name}'");

            return Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #region Events

        internal ControlElement InternalTunnelMouseMoveEvent(int x, int y, ref bool handled)
        {
            var control = default(ControlElement);

            foreach (var m in _elements)
            {
                control = m.InternalTunnelMouseMoveEvent(x, y, ref handled);
                if (handled) break;
                if (control != null) break;
            }

            return control;
        }

        internal ControlElement InternalTunnelMouseOverEvent(int x, int y, ref bool handled)
        {
            var control = default(ControlElement);

            foreach (var m in _elements)
            {
                control = m.InternalTunnelMouseOverEvent(x, y, ref handled);
                if (handled) break;
                if (control != null) break;
            }

            return control;
        }

        internal ControlElement InternalTunnelMouseButtonUpEvent(int x, int y, MouseButton button, ref bool handled)
        {
            var control = default(ControlElement);

            foreach (var m in _elements)
            {
                control = m.InternalTunnelMouseButtonUpEvent(x, y, button, ref handled);
                if (handled) break;
                if (control != null) break;
            }

            return control;
        }

        internal ControlElement InternalTunnelMouseButtonDownEvent(int x, int y, MouseButton button, ref bool handled)
        {
            var control = default(ControlElement);

            foreach (var m in _elements)
            {
                control = m.InternalTunnelMouseButtonDownEvent(x, y, button, ref handled);
                if (handled) break;
                if (control != null) break;
            }

            return control;
        }

        #endregion
    }
}
