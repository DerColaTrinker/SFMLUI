using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML.Graphics;
using Pandora.SFML.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Controls.Primitives
{
    public enum UsageSpecifier
    {
        Stream,
        Dynamic,
        Static
    }

    public class VertexBuffer : UIElement
    {
        public static bool Available { get { return NativeSFML.sfVertexBuffer_isAvailable(); } }

        public VertexBuffer(uint vertexCount, PrimitiveType primitiveType, UsageSpecifier usageType) : base(NativeSFML.sfVertexBuffer_create(vertexCount, primitiveType, usageType))
        { }

        public VertexBuffer(VertexBuffer copy) : base(NativeSFML.sfVertexBuffer_copy(copy.Pointer))
        { }

        public uint VertexCount { get { return NativeSFML.sfVertexBuffer_getVertexCount(Pointer); } }

        public uint NativeHandle
        {
            get { return NativeSFML.sfVertexBuffer_getNativeHandle(Pointer); }
        }

        public PrimitiveType PrimitiveType
        {
            get { return NativeSFML.sfVertexBuffer_getPrimitiveType(Pointer); }
            set { NativeSFML.sfVertexBuffer_setPrimitiveType(Pointer, value); }
        }

        public UsageSpecifier Usage
        {
            get { return NativeSFML.sfVertexBuffer_getUsage(Pointer); }
            set { NativeSFML.sfVertexBuffer_setUsage(Pointer, value); }
        }

        public bool Update(Vertex[] vertices, uint vertexCount, uint offset)
        {
            unsafe
            {
                fixed (Vertex* verts = vertices)
                {
                    return NativeSFML.sfVertexBuffer_update(Pointer, verts, vertexCount, offset);
                }
            }
        }

        public bool Update(Vertex[] vertices)
        {
            return this.Update(vertices, (uint)vertices.Length, 0);
        }

        public bool Update(Vertex[] vertices, uint offset)
        {
            return this.Update(vertices, (uint)vertices.Length, offset);
        }

        public bool Update(VertexBuffer other)
        {
            return NativeSFML.sfVertexBuffer_updateFromVertexBuffer(Pointer, other.Pointer);
        }

        public void Swap(VertexBuffer other)
        {
            NativeSFML.sfVertexBuffer_swap(Pointer, other.Pointer);
        }

        protected override void Destroy(bool disposing)
        {
            NativeSFML.sfVertexArray_destroy(Pointer);
        }

        internal override void InternalRenderUpdate(RenderTargetBase target)
        {
            var states = RenderStates.Default;
            states.Transform *= Transform;
            var marshaledStates = states.Marshal();

            target.DrawVertexBuffer(Pointer, ref marshaledStates);
        }
    }
}
