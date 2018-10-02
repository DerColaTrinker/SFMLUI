using Pandora.Interactions.Controller;
using Pandora.Interactions.UI;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    internal sealed class EventDispatcher
    {
        private IntPtr _pointer;
        private ControlElement _dbclickcontrol;
        private DateTime _dbclicktimestamp;

        public double DoubleClickTime { get; private set; }

        public ControlElement FocusControl { get; private set; }

        internal EventDispatcher(SceneHandler handler)
        {
            _pointer = handler.Pointer;
            DoubleClickTime = 1000;
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

        internal bool PollEvent(out Event eventToFill)
        {
            return NativeSFML.sfWindow_pollEvent(_pointer, out eventToFill);
        }

        internal void RenderUpdate(RenderTargetBase target)
        {
            Render?.Invoke(target);
        }

        internal bool WaitEvent(out Event eventToFill)
        {
            return NativeSFML.sfWindow_waitEvent(_pointer, out eventToFill);
        }

        public bool CheckDoubleClick(ControlElement control)
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

        public void SetFocusControl(ControlElement control)
        {
            if (control != FocusControl)
            {
                // Ein anderes Steuerelement erzwingt den Fokus
                FocusControl?.InternalLostFocus();
                FocusControl = control;    // Auch wenn control null ist...

                // Fokus nur setzen wenn es auch erlaubt ist.
                if (control != null && control.AllowFocus) control.InternalGetFocus();
            }
        }

        private void CallEventHandler(Event e)
        {
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

                case EventType.LostFocus:
                    LostFocus?.Invoke();
                    break;

                case EventType.MouseButtonPressed:
                    MouseButtonDown?.Invoke(e.MouseButton.Button, e.MouseButton.X, e.MouseButton.Y);
                    break;

                case EventType.MouseButtonReleased:
                    MouseButtonUp?.Invoke(e.MouseButton.Button, e.MouseButton.X, e.MouseButton.Y);
                    break;

                case EventType.MouseEntered:
                    WindowMouseEnter?.Invoke();
                    break;

                case EventType.MouseLeft:
                    WindowMouseLeft?.Invoke();
                    break;

                case EventType.MouseMoved:
                    MouseMove?.Invoke(e.MouseMove.X, e.MouseMove.Y);
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

        public event DispatcherRenderUpdateDelegate Render;

        public event DispatcherWindowDelegate Closed;

        public event DispatcherWindowResizeDelegate WindowResized;

        public event DispatcherWindowDelegate LostFocus;

        public event DispatcherWindowDelegate GetFocus;

        public event DispatcherKeyDelegate KeyUp;

        public event DispatcherShortcutKeyDelegate ShortCutKeyUp;

        public event DispatcherKeyDelegate KeyDown;

        public event DispatcherMouseWheelMoveDelegate MouseWheelMove;

        public event DispatcherMouseButtonDelegate MouseButtonUp;

        public event DispatcherMouseButtonDelegate MouseButtonDown;

        public event DispatcherMouseMoveDelegate MouseMove;

        public event DispatcherWindowDelegate WindowMouseEnter;

        public event DispatcherWindowDelegate WindowMouseLeft;
    }
}
