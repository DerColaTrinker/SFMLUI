using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing.Shader
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IVec4
    {
        public IVec4(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public IVec4(Color color)
        {
            X = color.R;
            Y = color.G;
            Z = color.B;
            W = color.A;
        }

        public int X;

        public int Y;

        public int Z;

        public int W;
    }
}
