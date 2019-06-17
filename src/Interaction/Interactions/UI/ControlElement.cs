using Pandora.Interactions.Bindings;
using Pandora.Interactions.Controller;
using Pandora.Interactions.UI.Design;
using System;

namespace Pandora.Interactions.UI
{
    public delegate void EventDelegate(ControlElement element);

    public delegate void PreMouseMoveDelegate(ControlElement element, float x, float y, ref bool handled);
    public delegate void MouseMoveDelegate(ControlElement element, float x, float y);

    public delegate void PreMouseWheelMoveDelegate(ControlElement element, ref bool cancel, MouseWheel wheel, float delta, float x, float y);
    public delegate void MouseWheelMoveDelegate(ControlElement element, MouseWheel wheel, float delta, float x, float y);
    public delegate void PreMouseButtonUpDelegate(ControlElement element, MouseButton button, float x, float y, ref bool handled);
    public delegate void PreMouseButtonDownDelegate(ControlElement element, MouseButton button, float x, float y, ref bool handled);
    public delegate void MouseButtonUpDelegate(ControlElement element, MouseButton button, float x, float y);
    public delegate void MouseButtonDownDelegate(ControlElement element, MouseButton button, float x, float y);

    public delegate void ControlKeyDelegate(ControlElement element, bool control, bool alt, bool shift, bool system, KeyboardKey key);
    public delegate void ControlKeyPressDelegate(ControlElement element, char unicode);

    public abstract class ControlElement : UIElement, ITemplate
    {
        protected ControlElement() : base(IntPtr.Zero)
        {
            Animations = new EffectCollection();
            Templates = new UITemplateCollection(this);

            DesignHandler.ApplyDesignToControl(this);

            RegisterBindingProperties();
        }

        private void RegisterBindingProperties()
        {
            EnabledBinding = Bindings.Create("Enabled", true);
        }

        public BindingProperty<bool> EnabledBinding { get; private set; }

        internal override void InternalOnLoad(SceneHandler handler)
        {
            base.InternalOnLoad(handler);

            RegisterDispatcherEvents();
        }

        public UITemplateCollection Templates { get; private set; }

        public new ControlElement Parent { get { return (ControlElement)base.Parent; } }

        public Scene FindScene()
        {
            var parent = Parent;

            while (parent != null)
            {
                if (parent is Scene scene) return scene;
                parent = parent.Parent;
            }

            return null;
        }

        public bool AllowFocus { get; protected set; }

        public bool IsFocus { get { return Dispatcher.FocusControl == this; } }

        public bool IsMouseOver { get { return Dispatcher.HooverControl == this; } }

        public bool Enabled { get { return EnabledBinding.Value; } set { EnabledBinding.Value = value; } }

        public EffectCollection Animations { get; internal set; }

        #region Events

        private void RegisterDispatcherEvents()
        {
            //Dispatcher.MouseMove += RegisterMouseMovedEvent;

            Dispatcher.MouseWheelMove += RegisterMouseWheelMoveEvent;

            Dispatcher.ShortCutKeyUp += RegisterKeyShortCutEvent;
            Dispatcher.KeyUp += RegisterKeyUp;
            Dispatcher.KeyDown += RegisterKeyDown;
        }

        #region MouseMove

        public event PreMouseMoveDelegate PreMouseMove;
        public event MouseMoveDelegate MouseMove;

        internal virtual ControlElement InternalTunnelMouseMoveEvent(int x, int y, ref bool handled)
        {
            if (Enabled && HitTest(x, y))
            {
                var control = default(ControlElement);

                if (this is IContainer container)
                {
                    control = container.Controls.InternalTunnelMouseMoveEvent(x, y, ref handled);
                    if (control != null) return control;
                }

                OnPreMouseMove(x, y, ref handled);
                return this;
            }

            return null;
        }

        internal virtual void InternalMouseMoveEvent(int x, int y)
        {
            OnMouseMove(x, y);
        }

        protected virtual void OnPreMouseMove(float x, float y, ref bool handled)
        {
            PreMouseMove?.Invoke(this, x, y, ref handled);
        }

        protected virtual void OnMouseMove(float x, float y)
        {
            MouseMove?.Invoke(this, x, y);
        }

        #endregion

        #region MouseOver

        public event PreMouseMoveDelegate PreMouseOver;
        public event EventDelegate MouseEnter;
        public event EventDelegate MouseLeave;

        internal virtual ControlElement InternalTunnelMouseOverEvent(int x, int y, ref bool handled)
        {
            if (Enabled && HitTest(x, y))
            {
                var control = default(ControlElement);

                if (this is IContainer container)
                {
                    control = container.Controls.InternalTunnelMouseOverEvent(x, y, ref handled);
                    if (control != null) return control;
                }

                OnPreMouseOver(x, y, ref handled);
                return this;
            }

            return null;
        }

        protected virtual void OnPreMouseOver(float x, float y, ref bool handled)
        {
            PreMouseOver?.Invoke(this, x, y, ref handled);
        }

        internal virtual void InternalMouseEnterEvent()
        {
            OnMouseEnter();
        }

        protected virtual void OnMouseEnter()
        {
            MouseEnter?.Invoke(this);
        }

        internal virtual void InternalMouseLeaveEvent()
        {
            OnMouseLeave();
        }

        protected virtual void OnMouseLeave()
        {
            MouseLeave?.Invoke(this);
        }

        #endregion

        #region MouseButton

        public event EventDelegate MouseClick;
        public event EventDelegate MouseDoubleClick;

        public event PreMouseButtonUpDelegate PreMouseButtonUp;
        public event PreMouseButtonDownDelegate PreMouseButtonDown;

        public event MouseButtonUpDelegate MouseButtonUp;
        public event MouseButtonDownDelegate MouseButtonDown;

        internal virtual ControlElement InternalTunnelMouseButtonUpEvent(int x, int y, MouseButton button, ref bool handled)
        {
            if (Enabled && HitTest(x, y))
            {
                var control = default(ControlElement);

                if (this is IContainer container)
                {
                    control = container.Controls.InternalTunnelMouseButtonUpEvent(x, y, button, ref handled);
                    if (control != null) return control;
                }

                OnPreMouseButtonUp(x, y, button, ref handled);
                return this;
            }

            return null;
        }

        protected virtual void OnPreMouseButtonUp(int x, int y, MouseButton button, ref bool handled)
        {
            PreMouseButtonUp?.Invoke(this, button, x, y, ref handled);
        }

        internal virtual ControlElement InternalTunnelMouseButtonDownEvent(int x, int y, MouseButton button, ref bool handled)
        {
            if (Enabled && HitTest(x, y))
            {
                var control = default(ControlElement);

                if (this is IContainer container)
                {
                    control = container.Controls.InternalTunnelMouseButtonDownEvent(x, y, button, ref handled);
                    if (control != null) return control;
                }

                OnPreMouseButtonDown(x, y, button, ref handled);
                return this;
            }

            return null;
        }

        protected virtual void OnPreMouseButtonDown(int x, int y, MouseButton button, ref bool handled)
        {
            PreMouseButtonDown?.Invoke(this, button, x, y, ref handled);
        }

        internal virtual void InternalMouseClickEvent()
        {
            OnMouseClick();
        }

        protected virtual void OnMouseClick()
        {
            MouseClick?.Invoke(this);
        }

        internal virtual void InternalMouseDoubleClickEvent()
        {
            OnMouseDoubleClick();
        }

        protected virtual void OnMouseDoubleClick()
        {
            MouseDoubleClick?.Invoke(this);
        }

        internal virtual void InternalMouseButtonDownEvent(MouseButton button, int x, int y)
        {
            OnMouseButtonDown(x, y, button);
        }

        protected virtual void OnMouseButtonDown(int x, int y, MouseButton button)
        {
            MouseButtonDown?.Invoke(this, button, x, y);
        }

        internal virtual void InternalMouseButtonUpEvent(MouseButton button, int x, int y)
        {
            OnMouseButtonUp(x, y, button);
        }

        protected virtual void OnMouseButtonUp(int x, int y, MouseButton button)
        {
            MouseButtonUp?.Invoke(this, button, x, y);
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

        public event EventDelegate GetFocus;
        public event EventDelegate LostFocus;
        public event ControlKeyDelegate KeyUpFocus;
        public event ControlKeyDelegate KeyDownFocus;
        public event ControlKeyPressDelegate KeyPressFocus;

        internal virtual void InternalLostFocusEvent()
        {
            OnLostFocus();
        }

        protected virtual void OnLostFocus()
        {
            LostFocus?.Invoke(this);
        }

        internal virtual void InternalGetFocusEvent()
        {
            OnGetFocus();
        }

        protected virtual void OnGetFocus()
        {
            GetFocus?.Invoke(this);
        }

        internal virtual void InternalFocusKeyUp(bool control, bool alt, bool shift, bool system, KeyboardKey key)
        {
            OnFocusKeyUp(control, alt, shift, system, key);
        }

        protected virtual void OnFocusKeyUp(bool control, bool alt, bool shift, bool system, KeyboardKey key)
        {
            KeyUpFocus?.Invoke(this, control, alt, shift, system, key);
        }

        internal virtual void InternalFocusKeyDown(bool control, bool alt, bool shift, bool system, KeyboardKey key)
        {
            OnFocusKeyDown(control, alt, shift, system, key);
        }

        protected virtual void OnFocusKeyDown(bool control, bool alt, bool shift, bool system, KeyboardKey key)
        {
            KeyDownFocus?.Invoke(this, control, alt, shift, system, key);
        }

        internal virtual void InternalFocusKeyPress(char unicode)
        {
            OnFocusKeyPress(unicode);
        }

        protected virtual void OnFocusKeyPress(char unicode)
        {
            KeyPressFocus?.Invoke(this, unicode);
        }

        #endregion

        #endregion
    }
}
