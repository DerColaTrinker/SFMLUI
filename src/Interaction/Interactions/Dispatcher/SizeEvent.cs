using System.Runtime.InteropServices;

namespace Pandora.Interactions.Dispatcher
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SizeEvent
    {
        public uint Width;

        public uint Height;
    }
}
