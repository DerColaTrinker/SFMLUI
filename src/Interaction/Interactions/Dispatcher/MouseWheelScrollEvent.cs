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
    public struct MouseWheelScrollEvent
    {
        public MouseWheel Wheel;

        public float Delta;

        public int X;

        public int Y;
    }
}
