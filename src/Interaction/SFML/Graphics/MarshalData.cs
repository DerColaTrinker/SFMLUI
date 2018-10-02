using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.SFML.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MarshalData
    {
        public BlendMode blendMode;
        public Transform transform;
        public IntPtr texture;
        public IntPtr shader;
    }
}
