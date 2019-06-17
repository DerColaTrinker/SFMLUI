using Pandora.SFML;
using Pandora.SFML.Native;
using System;

namespace Pandora.Interactions.UI.Drawing
{
    public enum CursorType
    {
        Arrow,
        ArrowWait,
        Wait,
        Text,
        Hand,
        SizeHorinzontal,
        SizeVertical,
        SizeTopLeftBottomRight,
        SizeBottomLeftTopRight,
        SizeAll,
        Cross,
        Help,
        NotAllowed
    }

    public class Cursor : ObjectPointer
    {
        public Cursor(CursorType type) : base(NativeSFML.sfCursor_createFromSystem(type))
        { }

        public Cursor(byte[] pixels, Vector2U size, Vector2U hotspot) : base((IntPtr)0)
        {
            unsafe
            {
                fixed (byte* ptr = pixels)
                {
                    Pointer = NativeSFML.sfCursor_createFromPixels((IntPtr)ptr, size, hotspot);
                }
            }
        }

        protected override void Destroy(bool disposing)
        {
            NativeSFML.sfCursor_destroy(Pointer);
        }
    }
}
