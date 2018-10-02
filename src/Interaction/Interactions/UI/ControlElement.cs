using Pandora.Interactions.Bindings;
using Pandora.Interactions.Controller;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using Pandora.Interactions.UI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI
{
    public delegate void EventDelegate(ControlElement element);

    public delegate void PreMouseMoveDelegate(ControlElement element, ref bool cancel, float x, float y);
    public delegate void MouseMoveDelegate(ControlElement element, float x, float y);

    public delegate void PreMouseWheelMoveDelegate(ControlElement element, ref bool cancel, MouseWheel wheel, float delta, float x, float y);
    public delegate void MouseWheelMoveDelegate(ControlElement element, MouseWheel wheel, float delta, float x, float y);

    public delegate void PreMouseButtonUpDelegate(ControlElement element, ref bool cancel, MouseButton button, float x, float y);
    public delegate void PreMouseButtonDownDelegate(ControlElement element, ref bool cancel, MouseButton button, float x, float y);
    public delegate void MouseButtonUpDelegate(ControlElement element, MouseButton button, float x, float y);
    public delegate void MouseButtonDownDelegate(ControlElement element, MouseButton button, float x, float y);

    public delegate void ControlKeyDelegate(ControlElement element, bool control, bool alt, bool shift, bool system, KeyboardKey key);

    public abstract class ControlElement : UIElement, ITemplate
    {
        private Style _style;

        protected ControlElement() : base(IntPtr.Zero)
        {
            InitializeUIElement();

            RegisterBindingProperties();
        }

        private void RegisterBindingProperties()
        {
            EnabledBinding = new BindingProperty<bool>("Enabled") { Value = true };
        }

        public BindingProperty<bool> EnabledBinding { get; private set; }

        internal override void InternalOnLoad(SceneHandler handler)
        {
            base.InternalOnLoad(handler);

            RegisterDispatcherEvents();
        }

        private void InitializeUIElement()
        {
            Templates = new UITemplateCollection(this);
        }

        public UITemplateCollection Templates { get; private set; }

        public bool AllowFocus { get; protected set; }

        public bool HasFocus { get { return Dispatcher.FocusControl == this; } }

        public bool Enabled { get { return EnabledBinding.Value; } set { EnabledBinding.Value = value; } }

        #region Events

        private void RegisterDispatcherEvents()
        {
            Dispatcher.MouseMove += RegisterMouseMovedEvent;
            Dispatcher.MouseWheelMove += RegisterMouseWheelMoveEvent;
            Dispatcher.MouseButtonUp += RegisterMouseButtonUpEvent;
            Dispatcher.MouseButtonDown += RegisterMouseButtonDownEvent;

            Dispatcher.ShortCutKeyUp += RegisterKeyShortCutEvent;
            Dispatcher.KeyUp += RegisterKeyUp;
            Dispatcher.KeyDown += RegisterKeyDown;
        }

        #region MouseMove

        public event PreMouseMoveDelegate PreMouseMove;
        public event MouseMoveDelegate MouseMove;

        private void RegisterMouseMovedEvent(float x, float y)
        {
            if (Enabled && MouseMove != null)
            {
                if (HitTest(x, y))
                {
                    var cancel = false;
                    OnPreMouseMove(ref cancel, x, y);
                    if (cancel) return;
                    MouseMove?.Invoke(this, x, y);
                }
            }
        }

        private void OnPreMouseMove(ref bool cancel, float x, float y)
        {
            PreMouseMove?.Invoke(this, ref cancel, x, y);
        }

        #endregion

        #region MouseButton

        public event EventDelegate MouseButtonDoubleClick;
        public event PreMouseButtonUpDelegate PreMouseButtonUp;
        public event PreMouseButtonDownDelegate PreMouseButtonDown;
        public event MouseButtonUpDelegate MouseButtonUp;
        public event MouseButtonDownDelegate MouseButtonDown;

        private void RegisterMouseButtonUpEvent(MouseButton button, float x, float y)
        {
            if (Enabled && MouseButtonUp != null)
            {
                if (HitTest(x, y))
                {
                    // Das Event wird im Dispatcher verwaltet.
                    if (Dispatcher.CheckDoubleClick(this))
                    {
                        MouseButtonDoubleClick?.Invoke(this);
                    }

                    Dispatcher.SetFocusControl(this);

                    var cancel = false;
                    OnPreMouseButtonUp(ref cancel, button, x, y);
                    if (cancel) return;
                    MouseButtonUp?.Invoke(this, button, x, y);
                }
            }
        }



        private void RegisterMouseButtonDownEvent(MouseButton button, float x, float y)
        {
            if (MouseButtonDown != null)
            {
                if (HitTest(x, y))
                {
                    var cancel = false;
                    OnPreMouseButtonDown(ref cancel, button, x, y);
                    if (cancel) return;
                    MouseButtonDown?.Invoke(this, button, x, y);
                }
            }
        }



        private void OnPreMouseButtonUp(ref bool cancel, MouseButton button, float x, float y)
        {
            PreMouseButtonUp?.Invoke(this, ref cancel, button, x, y);
        }

        private void OnPreMouseButtonDown(ref bool cancel, MouseButton button, float x, float y)
        {
            PreMouseButtonDown?.Invoke(this, ref cancel, button, x, y);
        }



        #endregion

        #region MouseWheel

        public event PreMouseWheelMoveDelegate PreMouseWheelMove;
        public event MouseWheelMoveDelegate MouseWheelMove;

        private void RegisterMouseWheelMoveEvent(MouseWheel wheel, float delta, float x, float y)
        {
            if (Enabled && MouseMove != null)
            {
                if (HitTest(x, y))
                {
                    var cancel = false;
                    OnPreMouseWheelMove(ref cancel, wheel, delta, x, y);
                    if (cancel) return;
                    MouseWheelMove?.Invoke(this, wheel, delta, x, y);
                }
            }
        }

        private void OnPreMouseWheelMove(ref bool cancel, MouseWheel wheel, float delta, float x, float y)
        {
            PreMouseWheelMove?.Invoke(this, ref cancel, wheel, delta, x, y);
        }

        #endregion

        #region Shortcut KeyStroke, KeyUp, KeyDown

        public event ControlKeyDelegate KeyUp;
        public event ControlKeyDelegate KeyDown;

        private void RegisterKeyShortCutEvent(bool control, bool alt, bool shift, bool system, KeyboardKey key, ref bool handled)
        {
            OnShortCutKeyPress(control, alt, shift, system, key, ref handled);
        }

        protected virtual void OnShortCutKeyPress(bool control, bool alt, bool shift, bool system, KeyboardKey key, ref bool handled)
        {
            // Leer lassen. Nichts machen!
        }

        private void RegisterKeyDown(bool control, bool alt, bool shift, bool system, KeyboardKey key)
        {
            KeyDown?.Invoke(this, control, alt, shift, system, key);
        }

        private void RegisterKeyUp(bool control, bool alt, bool shift, bool system, KeyboardKey key)
        {
            KeyUp?.Invoke(this, control, alt, shift, system, key);
        }

        #endregion

        #region Focus

        public event EventDelegate ControlGetFocus;
        public event EventDelegate ControlLostFocus;

        internal void InternalLostFocus()
        {
            ControlLostFocus?.Invoke(this);
        }

        internal void InternalGetFocus()
        {
            ControlGetFocus?.Invoke(this);
        }

        internal void InternalFocusKeyUp(bool control, bool alt, bool shift, bool system, KeyboardKey key)
        {

        }

        internal void InternalFocusKeyDown(bool control, bool alt, bool shift, bool system, KeyboardKey key)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region Style

        public Style Style
        {
            get { return _style; }
            set
            {
                StyleHandler.ApplyStyle(this, value);
                _style = value;
            }
        }

        public string Stylename
        {
            get { return _style == null ? string.Empty : _style.Name; }
            set
            {
                Style = StyleHandler.GetStyle(value);
            }
        }

        #endregion
    }
}
