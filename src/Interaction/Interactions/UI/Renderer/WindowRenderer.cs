using Pandora.Interactions.UI.Drawing;
using Pandora.SFML;
using Pandora.SFML.Graphics;
using Pandora.SFML.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Renderer
{
    public sealed class WindowRenderer : RenderTargetBase
    {
        internal WindowRenderer(ObjectPointer pointer) : base(pointer.Pointer)
        { }

        #region Renderer Implementierung

        public override Vector2U Size
        {
            get { return NativeSFML.sfRenderWindow_getSize(Pointer); }
            set { NativeSFML.sfRenderWindow_setSize(Pointer, value); }
        }

        public override View DefaultView
        {
            get { return new View(NativeSFML.sfRenderWindow_getDefaultView(Pointer)); }
        }

        public override View View
        {
            get { return new View(NativeSFML.sfRenderWindow_getView(Pointer)); }
            set { NativeSFML.sfRenderWindow_setView(Pointer, value.Pointer); }
        }

        public override void Clear()
        {
            NativeSFML.sfRenderWindow_clear(Pointer, Color.Black);
        }

        public override void Clear(Color color)
        {
            NativeSFML.sfRenderWindow_clear(Pointer, color);
        }

        public override void Display()
        {
            NativeSFML.sfRenderWindow_display(Pointer);
        }

        public override void DrawShape(IntPtr pointer, ref MarshalData state)
        {
            NativeSFML.sfRenderWindow_drawShape(Pointer, pointer, ref state);
        }

        public override void DrawText(IntPtr pointer, ref MarshalData state)
        {
            NativeSFML.sfRenderWindow_drawText(Pointer, pointer, ref state);
        }

        public override void DrawVertex(IntPtr pointer, ref MarshalData states)
        {
            NativeSFML.sfRenderWindow_drawVertexArray(Pointer, pointer, ref states);
        }

        public override void DrawSprite(IntPtr pointer, ref MarshalData states)
        {
            NativeSFML.sfRenderWindow_drawSprite(Pointer, pointer, ref states);
        }

        public override Rectangle GetViewport(View view)
        {
            return NativeSFML.sfRenderWindow_getViewport(Pointer, view.Pointer);
        }

        public override Vector2 MapCoordsToPixel(Vector2F point)
        {
            return NativeSFML.sfRenderWindow_mapCoordsToPixel(Pointer, point, View.Pointer);
        }

        public override Vector2 MapCoordsToPixel(Vector2F point, View view)
        {
            return NativeSFML.sfRenderWindow_mapCoordsToPixel(Pointer, point, view.Pointer);
        }

        public override Vector2F MapPixelToCoords(Vector2 point)
        {
            return NativeSFML.sfRenderWindow_mapPixelToCoords(Pointer, point, View.Pointer);
        }

        public override Vector2F MapPixelToCoords(Vector2 point, View view)
        {
            return NativeSFML.sfRenderWindow_mapPixelToCoords(Pointer, point, view.Pointer);
        }

        protected override void Destroy(bool disposing)
        {
            NativeSFML.sfRenderWindow_destroy(Pointer);
        }

        #endregion
    }
}
