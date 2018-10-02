using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing.Shader
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BVec3
    {
        public BVec3(bool x, bool y, bool z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool X;

        public bool Y;

        public bool Z;
    }
}
