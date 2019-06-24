using Pandora.Interactions.Controller;
using Pandora.Interactions.UI;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML.Native;
using System;
using System.Runtime.InteropServices;

namespace Pandora.Interactions.Dispatcher
{
    internal delegate void DispatcherWindowDelegate();
    internal delegate void DispatcherWindowResizeDelegate(uint x, uint y);
    internal delegate void DispatcherRenderUpdateDelegate(RenderTargetBase target);
    internal delegate void DispatcherMouseMoveDelegate(float x, float y);
    internal delegate void DispatcherMouseButtonDelegate(MouseButton button, float x, float y);
    internal delegate void DispatcherMouseWheelMoveDelegate(MouseWheel wheel, float delta, float x, float y);
    internal delegate void DispatcherShortcutKeyDelegate(bool control, bool alt, bool shift, bool system, KeyboardKey key, ref bool handled);
    internal delegate void DispatcherKeyDelegate(bool control, bool alt, bool shift, bool system, KeyboardKey key);
    internal delegate void DispatcherTextDelegate(char unicode);

    internal sealed class EventDispatcher
    {
        private readonly SceneHandler _scenehandler;
        private ControlElement _dbclickcontrol;
        private DateTime _dbclicktimestamp;

        public double DoubleClickTime { get; private set; }

        public ControlElement FocusControl { get; private set; }

        public ControlElement HooverControl { get; private set; }
        public ControlElement MouseDownControl { get; private set; }

        #region Native

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern int GetDoubleClickTime();

        #endregion

        internal EventDispatcher(SceneHandler handler)
        {
            Logger.Debug("[Interaction] Create EventDispatcher instance");

            _scenehandler = handler;

            DoubleClickTime = GetDoubleClickTime();
        }

        public void WaitAndDispatchEvents()
        {
            if (WaitEvent(out Event e))
                CallEventHandler(e);
        }

        public void DispatchEvents()
        {
            while (PollEvent(out Event e))
                CallEventHandler(e);
        }

        internal bool PollEvent(out Event currentevent)
        {
            return NativeSFML.sfWindow_pollEvent(_scenehandler.Pointer, out currentevent);
        }

        internal void RenderUpdate(RenderTargetBase target)
        {
            Render?.Invoke(target);
        }

        internal bool WaitEvent(out Event currentevent)
        {
            return NativeSFML.sfWindow_waitEvent(_scenehandler.Pointer, out currentevent);
        }

        private void CallEventHandler(Event e)
        {
            Logger.Trace($"Dispatch event '{e.Type}'");

            switch (e.Type)
            {
                case EventType.Closed:
                    Closed?.Invoke();
                    break;

                case EventType.GainedFocus:
                    GetFocus?.Invoke();
                    break;

                case EventType.KeyPressed:
                    if (FocusControl == null)
                    {
                        KeyDown?.Invoke(e.Key.Control > 0, e.Key.Alt > 0, e.Key.Shift > 0, e.Key.System > 0, e.Key.Code);
                    }
                    else
                    {
                        FocusControl.InternalFocusKeyDown(e.Key.Control > 0, e.Key.Alt > 0, e.Key.Shift > 0, e.Key.System > 0, e.Key.Code);
                    }
                    break;

                case EventType.KeyReleased:
                    if (FocusControl == null)
                    {
                        var handled = false;
                        ShortCutKeyUp?.Invoke(e.Key.Control > 0, e.Key.Alt > 0, e.Key.Shift > 0, e.Key.System > 0, e.Key.Code, ref handled);
                        if (!handled) KeyUp?.Invoke(e.Key.Control > 0, e.Key.Alt > 0, e.Key.Shift > 0, e.Key.System > 0, e.Key.Code);
                    }
                    else
                    {
                        FocusControl.InternalFocusKeyUp(e.Key.Control > 0, e.Key.Alt > 0, e.Key.Shift > 0, e.Key.System > 0, e.Key.Code);
                    }
                    break;

                case EventType.TextEntered:
                    if (FocusControl == null)
                    {
                        KeyPress?.Invoke((char)(int)e.Unicode);
                    }
                    else
                    {
                        FocusControl.InternalFocusKeyPress((char)(int)e.Unicode);
                    }

                    break;

                case EventType.LostFocus:
                    LostFocus?.Invoke();
                    break;

                case EventType.MouseButtonPressed:
                    HandleMouseButtonDownEvent(e.MouseButton.Button, e.MouseButton.X, e.MouseButton.Y);
                    break;

                case EventType.MouseButtonReleased:
                    HandleMouseButtonUpEvent(e.MouseButton.Button, e.MouseButton.X, e.MouseButton.Y);
                    break;

                case EventType.MouseEntered:
                    WindowMouseEnter?.Invoke();
                    break;

                case EventType.MouseLeft:
                    WindowMouseLeft?.Invoke();
                    break;

                case EventType.MouseMoved:
                    HandleMouseMoveEvent(e.MouseMove.X, e.MouseMove.Y);
                    break;

                case EventType.MouseWheelScrolled:
                    MouseWheelMove?.Invoke(e.MouseWheelScroll.Wheel, e.MouseWheelScroll.Delta, e.MouseWheelScroll.X, e.MouseWheelScroll.Y);
                    break;

                case EventType.Resized:
                    WindowResized?.Invoke(e.Size.Width, e.Size.Height);
                    break;

                default:
                    break;
            }
        }

        #region MouseMove and MouseOver

        private void HandleMouseMoveEvent(int x, int y)
        {
            // Search for the deepest control element that has received this event.
            var handled = false;
            var control = _scenehandler.Scenes.InternalTunnelMouseMoveEvent(x, y, ref handled);

            HandleMouseOverEvent(x, y);

            // Let the event come up again from the found control.
            while (control != null)
            {
                control.InternalMouseMoveEvent(x, y);
                control = control.Parent;
            }
        }

        private void HandleMouseOverEvent(int x, int y)
        {
            var handled = false;
            var control = _scenehandler.Scenes.InternalTunnelMouseOverEvent(x, y, ref handled);

            if (HooverControl == control) return;
            HandleMouseLeaveEvent(HooverControl);
            HooverControl = control;
            HandleMouseEnterEvent(HooverControl);
        }

        private void HandleMouseEnterEvent(ControlElement control)
        {
            while (control != null)
            {
                control.InternalMouseEnterEvent();
                control = control.Parent;
            }
        }

        private void HandleMouseLeaveEvent(ControlElement control)
        {
            while (control != null)
            {
                control.InternalMouseLeaveEvent();
                control = control.Parent;
            }
        }

        #endregion

        #region MouseButtonUp and MouseButtonDown

        private void HandleMouseButtonUpEvent(MouseButton button, int x, int y)
        {
            // Search for the deepest control element that has received this event.
            var handled = false;
            var control = _scenehandler.Scenes.InternalTunnelMouseButtonUpEvent(x, y, button, ref handled);

            // Trigger the MouseClick event only when the button is also released above the control where it was pressed.
            if (button == MouseButton.Left && control == MouseDownControl)
                HandleMouseClick(control);

            // Trigger a MouseUp at the control where the MouseDown event was triggered.
            if (control != MouseDownControl)
            {
                while (MouseDownControl != null)
                {
                    MouseDownControl.InternalMouseButtonUpEvent(button, x, y);
                    MouseDownControl = MouseDownControl.Parent;
                }

                MouseDownControl = null;
            }

            // Let the event come up again from the found control.
            while (control != null)
            {
                control.InternalMouseButtonUpEvent(button, x, y);
                control = control.Parent;
            }
        }

        private void HandleMouseButtonDownEvent(MouseButton button, int x, int y)
        {
            // Search for the deepest control element that has received this event.
            var handled = false;
            var control = _scenehandler.Scenes.InternalTunnelMouseButtonDownEvent(x, y, button, ref handled);

            MouseDownControl = control;

            // Let the event come up again from the found control.
            while (control != null)
            {
                control.InternalMouseButtonDownEvent(button, x, y);
                control = control.Parent;
            }
        }

        private void HandleMouseClick(ControlElement control)
        {
            control.InternalMouseClickEvent();

            HandleFocusEvent(control);

            // Doublecklick check
            if (CheckDoubleClick(control))
                control.InternalMouseDoubleClickEvent();
        }

        private bool CheckDoubleClick(ControlElement control)
        {
            if (_dbclickcontrol == control)
            {
                // Wenn das Steuerelement vorher schonmal geklickt wurde, prüfen ob es innerhalb des Zeitraumes wiederholt geklickt wurde.
                if (control.Enabled && (DateTime.Now - _dbclicktimestamp).TotalMilliseconds <= DoubleClickTime)
                {
                    _dbclicktimestamp = DateTime.Now;
                    return true;
                }
                else
                {
                    // Zeitpunkt nicht getroffen, also neu setzten
                    _dbclicktimestamp = DateTime.Now;
                }
            }
            else
            {
                // Control hat gewechselt, also Control und Zeitpunkt festlegen
                _dbclicktimestamp = DateTime.Now;
                _dbclickcontrol = control;
            }

            return false;
        }

        private void HandleFocusEvent(ControlElement control)
        {
            if (control != FocusControl)
            {
                FocusControl?.InternalLostFocusEvent();

                if (control.AllowFocus)
                {

                    FocusControl = control;
                    FocusControl?.InternalGetFocusEvent();
                }
                else
                {

                    FocusControl = null;
                }
            }
        }

        #endregion

        public event DispatcherRenderUpdateDelegate Render;

        public event DispatcherWindowDelegate Closed;

        public event DispatcherWindowResizeDelegate WindowResized;

        public event DispatcherWindowDelegate LostFocus;

        public event DispatcherWindowDelegate GetFocus;

        public event DispatcherKeyDelegate KeyUp;

        public event DispatcherShortcutKeyDelegate ShortCutKeyUp;

        public event DispatcherKeyDelegate KeyDown;

        public event DispatcherTextDelegate KeyPress;

        public event DispatcherMouseWheelMoveDelegate MouseWheelMove;

        // public event DispatcherMouseButtonDelegate MouseButtonUp;

        //public event DispatcherMouseButtonDelegate MouseButtonDown;

        //internal event DispatcherMouseMoveDelegate MouseMove;

        public event DispatcherWindowDelegate WindowMouseEnter;

        public event DispatcherWindowDelegate WindowMouseLeft;
    }
}
