using Pandora.Interactions.UI.Drawing;
using Pandora.SFML;
using Pandora.SFML.Graphics;
using System;

namespace Pandora.Interactions.UI.Renderer
{
    public abstract class RenderTargetBase : ObjectPointer
    {
        protected RenderTargetBase(IntPtr pointer) : base(pointer)
        { }

        public abstract Vector2U Size { get; set; }

        public abstract View DefaultView { get; }

        public abstract View View { get; set; }

        public abstract Rectangle GetViewport(View view);

        public abstract Vector2F MapPixelToCoords(Vector2 point);

        public abstract Vector2F MapPixelToCoords(Vector2 point, View view);

        public abstract Vector2 MapCoordsToPixel(Vector2F point);

        public abstract Vector2 MapCoordsToPixel(Vector2F point, View view);

        public abstract void Clear();

        public abstract void Clear(Color color);

        public abstract void Display();

        public abstract void DrawShape(IntPtr pointer, ref MarshalData states);

        public abstract void DrawText(IntPtr pointer, ref MarshalData states);

        public abstract void DrawVertexArray(IntPtr pointer, ref MarshalData states);

        public abstract void DrawVertexBuffer(IntPtr pointer, ref MarshalData marshaledStates);

        public abstract void DrawSprite(IntPtr pointer, ref MarshalData states);



        //public abstract void Draw(UIElement drawable);

        //public abstract void Draw(UIElement drawable, RenderStates states);

        //public abstract void Draw(Vertex[] vertices, PrimitiveType type);

        //public abstract void Draw(Vertex[] vertices, PrimitiveType type, RenderStates states);

        //public abstract void Draw(Vertex[] vertices, uint start, uint count, PrimitiveType type);

        //public abstract void Draw(Vertex[] vertices, uint start, uint count, PrimitiveType type, RenderStates states);
    }
}
