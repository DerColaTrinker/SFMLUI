using Pandora.SFML.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Renderer
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VideoMode
    {
        public VideoMode(uint width, uint height) :
            this(width, height, 32)
        { }

        public VideoMode(uint width, uint height, uint bpp)
        {
            Width = width;
            Height = height;
            BitsPerPixel = bpp;
        }

        public bool IsValid()
        {
            return NativeSFML.sfVideoMode_isValid(this);
        }

        public static VideoMode[] FullscreenModes
        {
            get
            {
                unsafe
                {
                    uint Count;
                    VideoMode* ModesPtr = NativeSFML.sfVideoMode_getFullscreenModes(out Count);
                    VideoMode[] Modes = new VideoMode[Count];
                    for (uint i = 0; i < Count; ++i)
                        Modes[i] = ModesPtr[i];

                    return Modes;
                }
            }
        }

        public static VideoMode DesktopMode
        {
            get { return NativeSFML.sfVideoMode_getDesktopMode(); }
        }

        public override string ToString()
        {
            return "[VideoMode]" + " Width(" + Width + ")" + " Height(" + Height + ")" + " BitsPerPixel(" + BitsPerPixel + ")";
        }

        public uint Width;

        public uint Height;

        public uint BitsPerPixel;
    }
}