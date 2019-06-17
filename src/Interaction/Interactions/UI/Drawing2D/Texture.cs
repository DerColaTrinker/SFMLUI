using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML;
using Pandora.SFML.Native;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Pandora.Interactions.UI.Drawing2D
{
    public class Texture : ObjectPointer
    {
        private readonly bool _external = false;

        public Texture(uint width, uint height) : base(NativeSFML.sfTexture_create(width, height))
        {
            if (Pointer == IntPtr.Zero)
                throw new Exception("texture");
        }

        public Texture(string filename) : this(filename, new Rectangle(0, 0, 0, 0))
        { }

        public Texture(string filename, Rectangle area) : base(NativeSFML.sfTexture_createFromFile(filename, ref area))
        {
            if (Pointer == IntPtr.Zero)
                throw new Exception("texture : " + filename);
        }

        public Texture(Stream stream) : this(stream, new Rectangle(0, 0, 0, 0))
        { }

        public Texture(Stream stream, Rectangle area) : base(IntPtr.Zero)
        {
            using (StreamAdaptor adaptor = new StreamAdaptor(stream))
            {
                Pointer = NativeSFML.sfTexture_createFromStream(adaptor.InputStreamPtr, ref area);
            }

            if (Pointer == IntPtr.Zero)
                throw new Exception("texture");
        }

        public Texture(Image image) : this(image, new Rectangle(0, 0, 0, 0))
        { }

        public Texture(Image image, Rectangle area) : base(NativeSFML.sfTexture_createFromImage(image.Pointer, ref area))
        {
            if (Pointer == IntPtr.Zero)
                throw new Exception("texture");
        }

        public Texture(byte[] bytes) : base(IntPtr.Zero)
        {
            GCHandle pin = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                Rectangle rect = new Rectangle(0, 0, 0, 0);
                Pointer = NativeSFML.sfTexture_createFromMemory(pin.AddrOfPinnedObject(), Convert.ToUInt64(bytes.Length), ref rect);
            }
            finally
            {
                pin.Free();
            }
            if (Pointer == IntPtr.Zero)
                throw new Exception("texture");
        }

        public Texture(Texture copy) : base(NativeSFML.sfTexture_copy(copy.Pointer))
        { }

        public uint NativeHandle
        {
            get { return NativeSFML.sfTexture_getNativeHandle(Pointer); }
        }

        public Image CopyToImage()
        {
            return new Image(NativeSFML.sfTexture_copyToImage(Pointer));
        }

        public void Update(Texture texture, uint x, uint y)
        {
            NativeSFML.sfTexture_updateFromTexture(Pointer, texture.Pointer, x, y);
        }

        public void Update(byte[] pixels)
        {
            Vector2U size = Size;
            Update(pixels, size.X, size.Y, 0, 0);
        }

        public void Update(byte[] pixels, uint width, uint height, uint x, uint y)
        {
            unsafe
            {
                fixed (byte* ptr = pixels)
                {
                    NativeSFML.sfTexture_updateFromPixels(Pointer, ptr, width, height, x, y);
                }
            }
        }

        public void Update(Image image)
        {
            Update(image, 0, 0);
        }

        public void Update(Image image, uint x, uint y)
        {
            NativeSFML.sfTexture_updateFromImage(Pointer, image.Pointer, x, y);
        }

        public void Swap(Texture right)
        {
            NativeSFML.sfTexture_swap(Pointer, right.Pointer);
        }

        //public void Update(SFML.Window.Window window)
        //{
        //    Update(window, 0, 0);
        //}

        //public void Update(SFML.Window.Window window, uint x, uint y)
        //{
        //    NativeSFML.sfTexture_updateFromWindow(Pointer, window.Pointer, x, y);
        //}

        //public void Update(RenderWindow window)
        //{
        //    Update(window, 0, 0);
        //}

        //public void Update(RenderWindow window, uint x, uint y)
        //{
        //    NativeSFML.sfTexture_updateFromRenderWindow(Pointer, window.Pointer, x, y);
        //}

        public bool GenerateMipmap()
        {
            return NativeSFML.sfTexture_generateMipmap(Pointer);
        }

        public bool Smooth
        {
            get { return NativeSFML.sfTexture_isSmooth(Pointer); }
            set { NativeSFML.sfTexture_setSmooth(Pointer, value); }
        }

        public bool Srgb
        {
            get { return NativeSFML.sfTexture_isSrgb(Pointer); }
            set { NativeSFML.sfTexture_setSrgb(Pointer, value); }
        }

        public bool Repeated
        {
            get { return NativeSFML.sfTexture_isRepeated(Pointer); }
            set { NativeSFML.sfTexture_setRepeated(Pointer, value); }
        }

        public Vector2U Size
        {
            get { return NativeSFML.sfTexture_getSize(Pointer); }
        }

        public static void Bind(Texture texture)
        {
            NativeSFML.sfTexture_bind(texture != null ? texture.Pointer : IntPtr.Zero);
        }

        public static uint MaximumSize
        {
            get { return NativeSFML.sfTexture_getMaximumSize(); }
        }

        public override string ToString()
        {
            return "[Texture]" + " Size(" + Size + ")" + " Smooth(" + Smooth + ")" + " Repeated(" + Repeated + ")";
        }

        internal Texture(IntPtr Pointer) : base(Pointer)
        {
            _external = true;
        }

        protected override void Destroy(bool disposing)
        {
            if (!_external)
            {
                if (!disposing)
                    Context.Global.SetActive(true);

                NativeSFML.sfTexture_destroy(Pointer);

                if (!disposing)
                    Context.Global.SetActive(false);
            }
        }
    }
}
