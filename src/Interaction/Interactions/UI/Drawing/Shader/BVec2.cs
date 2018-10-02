using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing.Shader
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BVec2
    {
        public BVec2(bool x, bool y)
        {
            X = x;
            Y = y;
        }

        public bool X;

        public bool Y;
    }
}
