using System.Runtime.InteropServices;

namespace Pandora.Interactions.Dispatcher
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MouseMoveEvent
    {
        public int X;

        public int Y;
    }
}
