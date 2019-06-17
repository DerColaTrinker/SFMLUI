using System.Runtime.InteropServices;

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
