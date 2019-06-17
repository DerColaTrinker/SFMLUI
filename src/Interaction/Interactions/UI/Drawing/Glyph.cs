using System.Runtime.InteropServices;

namespace Pandora.Interactions.UI.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Glyph
    {
        public float Advance;

        public RectangleF Bounds;

        public Rectangle TextureRect;
    }
}
