using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing.Shader
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IVec3
    {
        public IVec3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X;

        public int Y;

        public int Z;
    }
}
