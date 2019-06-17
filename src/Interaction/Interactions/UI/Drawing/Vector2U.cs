using System;
using System.Runtime.InteropServices;

namespace Pandora.Interactions.UI.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2U : IEquatable<Vector2U>
    {
        public Vector2U(uint x, uint y)
        {
            X = x;
            Y = y;
        }

        public static double CrossProduct(Vector2U vector1, Vector2U vector2)
        {
            return vector1.X * vector2.Y - vector1.X * vector2.Y;
        }

        public static float AngleBetween(Vector2U vector1, Vector2U vector2)
        {
            double y = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double x = vector1.X * vector2.X + vector1.Y * vector2.Y;
            return (float)(Math.Atan2(y, x) * 57.295779513082323);
        }

        public static Vector2U operator -(Vector2U v1, Vector2U v2)
        {
            return new Vector2U(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2U operator +(Vector2U v1, Vector2U v2)
        {
            return new Vector2U(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2U operator *(Vector2U v, uint x)
        {
            return new Vector2U(v.X * x, v.Y * x);
        }

        public static Vector2U operator *(uint x, Vector2U v)
        {
            return new Vector2U(v.X * x, v.Y * x);
        }

        public static Vector2U operator /(Vector2U v, uint x)
        {
            return new Vector2U(v.X / x, v.Y / x);
        }

        public static bool operator ==(Vector2U v1, Vector2U v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector2U v1, Vector2U v2)
        {
            return !v1.Equals(v2);
        }

        public override string ToString()
        {
            return "[Vector2u]" + " X(" + X + ")" + " Y(" + Y + ")";
        }

        public override bool Equals(object obj)
        {
            return (obj is Vector2U) && Equals((Vector2U)obj);
        }

        public bool Equals(Vector2U other)
        {
            return (X == other.X) &&
                   (Y == other.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^
                   Y.GetHashCode();
        }

        public static implicit operator Vector2(Vector2U v)
        {
            return new Vector2((int)v.X, (int)v.Y);
        }

        public static implicit operator Vector2F(Vector2U v)
        {
            return new Vector2F((float)v.X, (float)v.Y);
        }

        public uint X;

        public uint Y;
    }
}
