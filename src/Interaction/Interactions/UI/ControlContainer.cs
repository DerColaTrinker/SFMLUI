using Pandora.Interactions.Controller;

namespace Pandora.Interactions.UI
{
    public abstract class ControlContainer : Control, IContainer
    {
        protected ControlContainer()
        {
            Controls = new ControlCollection(this);
        }

        public ControlCollection Controls { get; }

        #region Events

        internal override ControlElement InternalTunnelMouseMoveEvent(int x, int y, ref bool handled)
        {
            var control = Controls.InternalTunnelMouseMoveEvent(x, y, ref handled);
            if (handled) return control;

            return control ?? InternalTunnelMouseMoveEvent(x, y, ref handled);
        }

        internal override ControlElement InternalTunnelMouseOverEvent(int x, int y, ref bool handled)
        {
            var control = Controls.InternalTunnelMouseOverEvent(x, y, ref handled);
            if (handled) return control;

            return control ?? InternalTunnelMouseOverEvent(x, y, ref handled);
        }

        internal override ControlElement InternalTunnelMouseButtonUpEvent(int x, int y, MouseButton button, ref bool handled)
        {
            var control = Controls.InternalTunnelMouseButtonUpEvent(x, y, button, ref handled);
            if (handled) return control;

            return control ?? InternalTunnelMouseMoveEvent(x, y, ref handled);
        }

        internal override ControlElement InternalTunnelMouseButtonDownEvent(int x, int y, MouseButton button, ref bool handled)
        {
            var control = Controls.InternalTunnelMouseButtonDownEvent(x, y, button, ref handled);
            if (handled) return control;

            return control ?? InternalTunnelMouseMoveEvent(x, y, ref handled);
        }

        #endregion
    }
}

