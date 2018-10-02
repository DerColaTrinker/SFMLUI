using Pandora.Interactions.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
