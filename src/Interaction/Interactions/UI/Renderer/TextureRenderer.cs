using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Drawing2D;
using Pandora.SFML.Graphics;
using Pandora.SFML.Native;

namespace Pandora.Interactions.UI.Renderer
{
    public class TextureRenderer : RenderTargetBase
    {
        private View _defaultview;

        public TextureRenderer(uint width, uint height) : this(width, height, false)
        { }

        public TextureRenderer(uint width, uint height, bool depthbuffer) : base(NativeSFML.sfRenderTexture_create(width, height, depthbuffer))
        {
            _defaultview = new View(NativeSFML.sfRenderTexture_getDefaultView(Pointer));
            Texture = new Texture(NativeSFML.sfRenderTexture_getTexture(Pointer));
            GC.SuppressFinalize(_defaultview);
            GC.SuppressFinalize(Texture);
        }

        public bool SetActive(bool active)
        {
            return NativeSFML.sfRenderTexture_setActive(Pointer, active);
        }

        public bool Repeated
        {
            get { return NativeSFML.sfRenderTexture_isRepeated(Pointer); }
            set { NativeSFML.sfRenderTexture_setRepeated(Pointer, value); }
        }

        public override Vector2U Size
        {
            get { return NativeSFML.sfRenderTexture_getSize(Pointer); }
            set
            {
                throw new NotSupportedException("Resize on texture rendering not supported by sfml");
            }
        }

        public override View DefaultView
        {
            get { return new View(_defaultview); }
        }

        public override View View
        {
            get { return new View(NativeSFML.sfRenderTexture_getView(Pointer)); }
            set { NativeSFML.sfRenderTexture_setView(Pointer, value.Pointer); }
        }

        public override Rectangle GetViewport(View view)
        {
            return NativeSFML.sfRenderTexture_getViewport(Pointer, view.Pointer);
        }

        public override Vector2F MapPixelToCoords(Vector2 point)
        {
            return MapPixelToCoords(point, View);
        }

        public override Vector2F MapPixelToCoords(Vector2 point, View view)
        {
            return NativeSFML.sfRenderTexture_mapPixelToCoords(Pointer, point, view != null ? view.Pointer : IntPtr.Zero);
        }

        public override Vector2 MapCoordsToPixel(Vector2F point)
        {
            return MapCoordsToPixel(point, View);
        }

        public override Vector2 MapCoordsToPixel(Vector2F point, View view)
        {
            return NativeSFML.sfRenderTexture_mapCoordsToPixel(Pointer, point, view != null ? view.Pointer : IntPtr.Zero);
        }

        public bool GenerateMipmap()
        {
            return NativeSFML.sfRenderTexture_generateMipmap(Pointer);
        }

        public override void Clear()
        {
            NativeSFML.sfRenderTexture_clear(Pointer, Color.Black);
        }

        public override void Clear(Color color)
        {
            NativeSFML.sfRenderTexture_clear(Pointer, color);
        }

        public override void Display()
        {
            NativeSFML.sfRenderTexture_display(Pointer);
        }

        public Texture Texture
        {
            get;
            private set;
        }

        public bool Smooth
        {
            get { return NativeSFML.sfRenderTexture_isSmooth(Pointer); }
            set { NativeSFML.sfRenderTexture_setSmooth(Pointer, value); }
        }

        public override void DrawShape(IntPtr pointer, ref MarshalData state)
        {
            NativeSFML.sfRenderTexture_drawShape(Pointer, pointer, ref state);
        }

        public override void DrawText(IntPtr pointer, ref MarshalData state)
        {
            NativeSFML.sfRenderTexture_drawText(Pointer, pointer, ref state);
        }

        public override void DrawVertex(IntPtr pointer, ref MarshalData states)
        {
            NativeSFML.sfRenderTexture_drawVertexArray(Pointer, pointer, ref states);
        }

        public override void DrawSprite(IntPtr pointer, ref MarshalData states)
        {
            NativeSFML.sfRenderTexture_drawSprite(Pointer, pointer, ref states);
        }

        public override string ToString()
        {
            return "[RenderTexture]" + " Size(" + Size + ")" + " Texture(" + Texture + ")" + " DefaultView(" + DefaultView + ")" + " View(" + View + ")";
        }

        protected override void Destroy(bool disposing)
        {
            if (!disposing)
                Context.Global.SetActive(true);

            NativeSFML.sfRenderTexture_destroy(Pointer);

            if (disposing)
            {
                _defaultview.Dispose();
                Texture.Dispose();
            }

            if (!disposing)
                Context.Global.SetActive(false);
        }
    }
}
