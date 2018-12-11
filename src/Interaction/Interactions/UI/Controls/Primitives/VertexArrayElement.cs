using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Controls.Primitives
{
    public class VertexArrayElement : UIElement
    {

        public VertexArrayElement() : base(NativeSFML.sfVertexArray_create())
        { }

        public VertexArrayElement(PrimitiveType type) : base(NativeSFML.sfVertexArray_create())
        {
            PrimitiveType = type;
        }

        public VertexArrayElement(PrimitiveType type, uint vertexCount) : base(NativeSFML.sfVertexArray_create())
        {
            PrimitiveType = type;
            Resize(vertexCount);
        }

        public VertexArrayElement(VertexArrayElement copy) : base(NativeSFML.sfVertexArray_copy(copy.Pointer))
        { }

        public uint VertexCount
        {
            get { return NativeSFML.sfVertexArray_getVertexCount(Pointer); }
        }

        public Vertex this[uint index]
        {
            get
            {
                unsafe
                {
                    return *NativeSFML.sfVertexArray_getVertex(Pointer, index);
                }
            }
            set
            {
                unsafe
                {
                    *NativeSFML.sfVertexArray_getVertex(Pointer, index) = value;
                }
            }
        }

        public void Clear()
        {
            NativeSFML.sfVertexArray_clear(Pointer);
        }

        public void Resize(uint vertexCount)
        {
            NativeSFML.sfVertexArray_resize(Pointer, vertexCount);

            InternalUpdateVertex();
        }

        protected override void ElementDimensionChange()
        {
            InternalUpdateVertex();
        }

        protected virtual void OnUpdateVertex(uint index)
        { }

        internal void InternalUpdateVertex()
        {
            for (uint i = 0; i < VertexCount; i++)
            {
                OnUpdateVertex(i);
            }
        }

        public void Append(Vertex vertex)
        {
            NativeSFML.sfVertexArray_append(Pointer, vertex);
        }

        public PrimitiveType PrimitiveType
        {
            get { return NativeSFML.sfVertexArray_getPrimitiveType(Pointer); }
            set { NativeSFML.sfVertexArray_setPrimitiveType(Pointer, value); }
        }

        public RectangleF Bounds
        {
            get { return NativeSFML.sfVertexArray_getBounds(Pointer); }
        }

        public override Vector2F Size
        {
            get { return base.Size; }
            set { base.Size = value; InternalUpdateVertex(); }
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

            target.DrawVertexArray(Pointer, ref marshaledStates);
        }
    }
}
