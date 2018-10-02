using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.SFML.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ContextSettings
    {
        public ContextSettings(uint depthBits, uint stencilBits) : this(depthBits, stencilBits, 0)
        { }

        public ContextSettings(uint depthBits, uint stencilBits, uint antialiasingLevel) : this(depthBits, stencilBits, antialiasingLevel, 2, 0, Attribute.Default, false)
        {
        }

        public ContextSettings(uint depthBits, uint stencilBits, uint antialiasingLevel, uint majorVersion, uint minorVersion, Attribute attributes, bool sRgbCapable)
        {
            DepthBits = depthBits;
            StencilBits = stencilBits;
            AntialiasingLevel = antialiasingLevel;
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            AttributeFlags = attributes;
            SRgbCapable = sRgbCapable;
        }

        public override string ToString()
        {
            return "[ContextSettings]" +
                   " DepthBits(" + DepthBits + ")" +
                   " StencilBits(" + StencilBits + ")" +
                   " AntialiasingLevel(" + AntialiasingLevel + ")" +
                   " MajorVersion(" + MajorVersion + ")" +
                   " MinorVersion(" + MinorVersion + ")" +
                   " AttributeFlags(" + AttributeFlags + ")";
        }

        public uint DepthBits;

        public uint StencilBits;

        public uint AntialiasingLevel;

        public uint MajorVersion;

        public uint MinorVersion;

        public Attribute AttributeFlags;

        public bool SRgbCapable;
    }
}