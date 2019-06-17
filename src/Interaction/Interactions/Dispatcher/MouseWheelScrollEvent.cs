using Pandora.Interactions.Controller;
using System.Runtime.InteropServices;

namespace Pandora.Interactions.Dispatcher
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseWheelScrollEvent
    {
        public MouseWheel Wheel;

        public float Delta;

        public int X;

        public int Y;
    }
}
