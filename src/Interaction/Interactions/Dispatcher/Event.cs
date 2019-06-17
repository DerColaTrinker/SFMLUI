using System.Runtime.InteropServices;

namespace Pandora.Interactions.Dispatcher
{
    [StructLayout(LayoutKind.Explicit, Size = 20)]
    internal struct Event
    {
        [FieldOffset(0)]
        public EventType Type;

        [FieldOffset(4)]
        public uint Unicode;

        [FieldOffset(4)]
        public SizeEvent Size;

        [FieldOffset(4)]
        public KeyEvent Key;

        [FieldOffset(4)]
        public MouseMoveEvent MouseMove;

        [FieldOffset(4)]
        public MouseButtonEvent MouseButton;

        [FieldOffset(4)]
        public MouseWheelScrollEvent MouseWheelScroll;
    }
}
