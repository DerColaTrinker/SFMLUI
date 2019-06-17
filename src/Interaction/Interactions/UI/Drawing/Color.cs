using System;
using System.Runtime.InteropServices;

namespace Pandora.Interactions.UI.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Color : IEquatable<Color>
    {
        public Color(byte red, byte green, byte blue) : this(red, green, blue, 255)
        { }

        public Color(byte red, byte green, byte blue, byte alpha)
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

        public Color(uint color)
        {
            unchecked
            {
                R = (byte)(color >> 24);
                G = (byte)(color >> 16);
                B = (byte)(color >> 8);
                A = (byte)color;
            }
        }

        public Color(Color color) :
            this(color.R, color.G, color.B, color.A)
        { }

        public uint ToInteger()
        {
            return (uint)((R << 24) | (G << 16) | (B << 8) | A);
        }

        public override string ToString()
        {
            return "[Color]" + " R(" + R + ")" + " G(" + G + ")" + " B(" + B + ")" + " A(" + A + ")";
        }

        public override bool Equals(object obj)
        {
            return (obj is Color) && Equals((Color)obj);
        }

        public bool Equals(Color other)
        {
            return (R == other.R) && (G == other.G) && (B == other.B) && (A == other.A);
        }

        public override int GetHashCode()
        {
            return (R << 24) | (G << 16) | (B << 8) | A;
        }

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

        public static Color operator +(Color left, Color right)
        {
            return new Color((byte)Math.Min(left.R + right.R, 255),
                             (byte)Math.Min(left.G + right.G, 255),
                             (byte)Math.Min(left.B + right.B, 255),
                             (byte)Math.Min(left.A + right.A, 255));
        }

        public static Color operator -(Color left, Color right)
        {
            return new Color((byte)Math.Max(left.R - right.R, 0),
                             (byte)Math.Max(left.G - right.G, 0),
                             (byte)Math.Max(left.B - right.B, 0),
                             (byte)Math.Max(left.A - right.A, 0));
        }

        public static Color operator *(Color left, Color right)
        {
            return new Color((byte)((int)left.R * right.R / 255),
                             (byte)((int)left.G * right.G / 255),
                             (byte)((int)left.B * right.B / 255),
                             (byte)((int)left.A * right.A / 255));
        }

        public byte R;

        public byte G;

        public byte B;

        public byte A;

        public static readonly Color Black = new Color(0, 0, 0);

        public static readonly Color White = new Color(255, 255, 255);

        public static readonly Color Red = new Color(255, 0, 0);

        public static readonly Color Green = new Color(0, 255, 0);

        public static readonly Color Blue = new Color(0, 0, 255);

        public static readonly Color Yellow = new Color(255, 255, 0);

        public static readonly Color Magenta = new Color(255, 0, 255);

        public static readonly Color Cyan = new Color(0, 255, 255);

        public static readonly Color Transparent = new Color(0, 0, 0, 0);

        public static readonly Color Gray = new Color(128, 128, 128);
    }
}
