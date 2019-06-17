using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using System;
using System.Runtime.InteropServices;

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
