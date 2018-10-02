using Pandora.Interactions.UI.Drawing;
using Pandora.SFML;
using Pandora.SFML.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Renderer
{
    public class View : ObjectPointer
    {
        private readonly bool _external = false;

        internal View(IntPtr cPointer) : base(cPointer)
        {
            _external = true;
        }

        public View() : base(NativeSFML.sfView_create())
        { }

        public View(RectangleF viewRect) : base(NativeSFML.sfView_createFromRect(viewRect))
        { }

        public View(Vector2F center, Vector2F size) : base(NativeSFML.sfView_create())
        {
            Center = center;
            Size = size;
        }

        public View(View copy) : base(NativeSFML.sfView_copy(copy.Pointer))
        { }

        public Vector2F Center
        {
            get { return NativeSFML.sfView_getCenter(Pointer); }
            set { NativeSFML.sfView_setCenter(Pointer, value); }
        }

        public Vector2F Size
        {
            get { return NativeSFML.sfView_getSize(Pointer); }
            set { NativeSFML.sfView_setSize(Pointer, value); }
        }

        public float Rotation
        {
            get { return NativeSFML.sfView_getRotation(Pointer); }
            set { NativeSFML.sfView_setRotation(Pointer, value); }
        }

        public RectangleF Viewport
        {
            get { return NativeSFML.sfView_getViewport(Pointer); }
            set { NativeSFML.sfView_setViewport(Pointer, value); }
        }

        public void Reset(RectangleF rectangle)
        {
            NativeSFML.sfView_reset(Pointer, rectangle);
        }

        public void Move(Vector2F offset)
        {
            NativeSFML.sfView_move(Pointer, offset);
        }

        public void Rotate(float angle)
        {
            NativeSFML.sfView_rotate(Pointer, angle);
        }

        public void Zoom(float factor)
        {
            NativeSFML.sfView_zoom(Pointer, factor);
        }

        public override string ToString()
        {
            return "[View]" + " Center(" + Center + ")" + " Size(" + Size + ")" + " Rotation(" + Rotation + ")" + " Viewport(" + Viewport + ")";
        }
        
        protected override void Destroy(bool disposing)
        {
            if (!_external)
                NativeSFML.sfView_destroy(Pointer);
        }
    }
}
