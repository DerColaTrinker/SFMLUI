using Pandora.Interactions.UI.Drawing;
using Pandora.SFML;
using Pandora.SFML.Native;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Pandora.Interactions.UI.Drawing2D
{
    public class Image : ObjectPointer
    {
        internal Image(IntPtr Pointer) : base(Pointer)
        {
        }

        public Image(uint width, uint height) : this(width, height, Color.Black)
        { }

        public Image(uint width, uint height, Color color) : base(NativeSFML.sfImage_createFromColor(width, height, color))
        {
            if (Pointer == IntPtr.Zero)
                throw new Exception("image");
        }

        public Image(string filename) : base(NativeSFML.sfImage_createFromFile(filename))
        {
            if (Pointer == IntPtr.Zero)
                throw new Exception("image : " + filename);
        }

        public Image(Stream stream) :
            base(IntPtr.Zero)
        {
            using (StreamAdaptor adaptor = new StreamAdaptor(stream))
            {
                Pointer = NativeSFML.sfImage_createFromStream(adaptor.InputStreamPtr);
            }

            if (Pointer == IntPtr.Zero)
                throw new Exception("image");
        }

        public Image(byte[] bytes) : this(new MemoryStream(bytes))
        { }

        public Image(Color[,] pixels) : base(IntPtr.Zero)
        {
            uint Width = (uint)pixels.GetLength(0);
            uint Height = (uint)pixels.GetLength(1);

            // Transpose the array (.Net gives dimensions in reverse order of what SFML expects)
            Color[,] transposed = new Color[Height, Width];
            for (int x = 0; x < Width; ++x)
                for (int y = 0; y < Height; ++y)
                    transposed[y, x] = pixels[x, y];

            unsafe
            {
                fixed (Color* PixelsPtr = transposed)
                {
                    Pointer = NativeSFML.sfImage_createFromPixels(Width, Height, (byte*)PixelsPtr);
                }
            }

            if (Pointer == IntPtr.Zero)
                throw new Exception("image");
        }

        public Image(uint width, uint height, byte[] pixels) : base(IntPtr.Zero)
        {
            unsafe
            {
                fixed (byte* PixelsPtr = pixels)
                {
                    Pointer = NativeSFML.sfImage_createFromPixels(width, height, PixelsPtr);
                }
            }

            if (Pointer == IntPtr.Zero)
                throw new Exception("image");
        }

        public Image(Image copy) : base(NativeSFML.sfImage_copy(copy.Pointer))
        { }

        public bool SaveToFile(string filename)
        {
            return NativeSFML.sfImage_saveToFile(Pointer, filename);
        }

        public void CreateMaskFromColor(Color color)
        {
            CreateMaskFromColor(color, 0);
        }

        public void CreateMaskFromColor(Color color, byte alpha)
        {
            NativeSFML.sfImage_createMaskFromColor(Pointer, color, alpha);
        }

        public void Copy(Image source, uint destX, uint destY)
        {
            Copy(source, destX, destY, new Rectangle(0, 0, 0, 0));
        }

        public void Copy(Image source, uint destX, uint destY, Rectangle sourceRect)
        {
            Copy(source, destX, destY, sourceRect, false);
        }

        public void Copy(Image source, uint destX, uint destY, Rectangle sourceRect, bool applyAlpha)
        {
            NativeSFML.sfImage_copyImage(Pointer, source.Pointer, destX, destY, sourceRect, applyAlpha);
        }

        public Color GetPixel(uint x, uint y)
        {
            return NativeSFML.sfImage_getPixel(Pointer, x, y);
        }

        public void SetPixel(uint x, uint y, Color color)
        {
            NativeSFML.sfImage_setPixel(Pointer, x, y, color);
        }

        public byte[] Pixels
        {
            get
            {
                Vector2U size = Size;
                byte[] PixelsPtr = new byte[size.X * size.Y * 4];
                Marshal.Copy(NativeSFML.sfImage_getPixelsPtr(Pointer), PixelsPtr, 0, PixelsPtr.Length);
                return PixelsPtr;
            }
        }

        public Vector2U Size
        {
            get { return NativeSFML.sfImage_getSize(Pointer); }
        }

        public void FlipHorizontally()
        {
            NativeSFML.sfImage_flipHorizontally(Pointer);
        }

        public void FlipVertically()
        {
            NativeSFML.sfImage_flipVertically(Pointer);
        }

        public override string ToString()
        {
            return "[Image]" + " Size(" + Size + ")";
        }

        protected override void Destroy(bool disposing)
        {
            NativeSFML.sfImage_destroy(Pointer);
        }
    }
}
