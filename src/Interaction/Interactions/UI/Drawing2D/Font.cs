using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML;
using Pandora.SFML.Graphics;
using Pandora.SFML.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Pandora.Interactions.UI.Drawing2D
{
    public class Font : ObjectPointer
    {
        private readonly Dictionary<uint, Texture> myTextures = new Dictionary<uint, Texture>();
        private readonly StreamAdaptor myStream = null;

        private Font(IntPtr Pointer) : base(Pointer)
        { }

        public Font(string filename) : base(NativeSFML.sfFont_createFromFile(filename))
        {
            if (Pointer == IntPtr.Zero)
                throw new Exception("font : " + filename);
        }

        public Font(Stream stream) : base(IntPtr.Zero)
        {
            myStream = new StreamAdaptor(stream);
            Pointer = NativeSFML.sfFont_createFromStream(myStream.InputStreamPtr);

            if (Pointer == IntPtr.Zero)
                throw new Exception("font");
        }

        public Font(byte[] bytes) : base(IntPtr.Zero)
        {
            GCHandle pin = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                Pointer = NativeSFML.sfFont_createFromMemory(pin.AddrOfPinnedObject(), Convert.ToUInt64(bytes.Length));
            }
            finally
            {
                pin.Free();
            }
            if (Pointer == IntPtr.Zero)
                throw new Exception("font");
        }

        public Font(Font copy) : base(NativeSFML.sfFont_copy(copy.Pointer))
        {
        }

        public Glyph GetGlyph(uint codePoint, uint characterSize, bool bold, float outlineThickness)
        {
            return NativeSFML.sfFont_getGlyph(Pointer, codePoint, characterSize, bold, outlineThickness);
        }

        public float GetKerning(uint first, uint second, uint characterSize)
        {
            return NativeSFML.sfFont_getKerning(Pointer, first, second, characterSize);
        }

        public float GetLineSpacing(uint characterSize)
        {
            return NativeSFML.sfFont_getLineSpacing(Pointer, characterSize);
        }

        public float GetUnderlinePosition(uint characterSize)
        {
            return NativeSFML.sfFont_getUnderlinePosition(Pointer, characterSize);
        }

        public float GetUnderlineThickness(uint characterSize)
        {
            return NativeSFML.sfFont_getUnderlineThickness(Pointer, characterSize);
        }

        public Texture GetTexture(uint characterSize)
        {
            myTextures[characterSize] = new Texture(NativeSFML.sfFont_getTexture(Pointer, characterSize));
            return myTextures[characterSize];
        }

        public Info GetInfo()
        {
            InfoMarshalData data = NativeSFML.sfFont_getInfo(Pointer);
            Info info = new Info
            {
                Family = Marshal.PtrToStringAnsi(data.Family)
            };

            return info;
        }

        public override string ToString()
        {
            return "[Font]";
        }

        protected override void Destroy(bool disposing)
        {
            if (!disposing)
                Context.Global.SetActive(true);

            NativeSFML.sfFont_destroy(Pointer);

            if (disposing)
            {
                foreach (Texture texture in myTextures.Values)
                    texture.Dispose();

                if (myStream != null)
                    myStream.Dispose();
            }

            if (!disposing)
                Context.Global.SetActive(false);
        }
    }
}
