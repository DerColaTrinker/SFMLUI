using Pandora.Interactions.Controller;
using System.Runtime.InteropServices;

namespace Pandora.Interactions.Dispatcher
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseButtonEvent
    {
        public MouseButton Button;

        public int X;

        public int Y;
    }

}
