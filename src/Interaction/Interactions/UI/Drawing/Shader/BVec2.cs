using System.Runtime.InteropServices;

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
