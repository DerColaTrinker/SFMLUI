using System;
using System.Runtime.InteropServices;

namespace Pandora.Interactions.UI.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2 : IEquatable<Vector2>
    {
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static double CrossProduct(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X * vector2.Y - vector1.X * vector2.Y;
        }

        public static float AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            double y = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double x = vector1.X * vector2.X + vector1.Y * vector2.Y;
            return (float)(Math.Atan2(y, x) * 57.295779513082323);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator *(Vector2 v, int x)
        {
            return new Vector2(v.X * x, v.Y * x);
        }

        public static Vector2 operator *(int x, Vector2 v)
        {
            return new Vector2(v.X * x, v.Y * x);
        }

        public static Vector2 operator /(Vector2 v, int x)
        {
            return new Vector2(v.X / x, v.Y / x);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !v1.Equals(v2);
        }

        public override string ToString()
        {
            return "[Vector2i]" +
                   " X(" + X + ")" +
                   " Y(" + Y + ")";
        }

        public override bool Equals(object obj)
        {
            return (obj is Vector2) && Equals((Vector2)obj);
        }

        public bool Equals(Vector2 other)
        {
            return (X == other.X) &&
                   (Y == other.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^
                   Y.GetHashCode();
        }

        public static implicit operator Vector2F(Vector2 v)
        {
            return new Vector2F((float)v.X, (float)v.Y);
        }

        public static implicit operator Vector2U(Vector2 v)
        {
            return new Vector2U((uint)v.X, (uint)v.Y);
        }

        public int X;

        public int Y;
    }
}
