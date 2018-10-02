using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Drawing2D;
using Pandora.SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Renderer
{
    public struct RenderStates
    {
        public RenderStates(BlendMode blendMode) : this(blendMode, Transform.Identity, null, null)
        { }

        public RenderStates(Transform transform) : this(BlendMode.Alpha, transform, null, null)
        { }

        public RenderStates(Texture texture) : this(BlendMode.Alpha, Transform.Identity, texture, null)
        { }

        public RenderStates(Shader shader) : this(BlendMode.Alpha, Transform.Identity, null, shader)
        { }

        public RenderStates(BlendMode blendMode, Transform transform, Texture texture, Shader shader)
        {
            BlendMode = blendMode;
            Transform = transform;
            Texture = texture;
            Shader = shader;
        }

        public RenderStates(RenderStates copy)
        {
            BlendMode = copy.BlendMode;
            Transform = copy.Transform;
            Texture = copy.Texture;
            Shader = copy.Shader;
        }

        public static RenderStates Default
        {
            get { return new RenderStates(BlendMode.Alpha, Transform.Identity, null, null); }
        }

        public BlendMode BlendMode;

        public Transform Transform;

        public Texture Texture;

        public Shader Shader;

        // Return a marshalled version of the instance, that can directly be passed to the C API
        internal MarshalData Marshal()
        {
            MarshalData data = new MarshalData
            {
                blendMode = BlendMode,
                transform = Transform,
                texture = Texture != null ? Texture.Pointer : IntPtr.Zero,
                shader = Shader != null ? Shader.Pointer : IntPtr.Zero
            };

            return data;
        }
    }
}
