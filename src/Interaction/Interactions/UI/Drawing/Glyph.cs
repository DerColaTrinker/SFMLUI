using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
