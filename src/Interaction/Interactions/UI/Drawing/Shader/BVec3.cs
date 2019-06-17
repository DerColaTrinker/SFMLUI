using System.Runtime.InteropServices;

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
