using Pandora.Interactions.Controller;
using System.Runtime.InteropServices;

namespace Pandora.Interactions.Dispatcher
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct KeyEvent
    {
        public KeyboardKey Code;

        public int Alt;

        public int Control;

        public int Shift;

        public int System;
    }
}
