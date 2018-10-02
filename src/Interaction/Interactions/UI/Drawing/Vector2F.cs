using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing
{
    public struct Vector2F : IEquatable<Vector2F>
    {
        public Vector2F(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        public static double CrossProduct(Vector2F vector1, Vector2F vector2)
        {
            return vector1.X * vector2.Y - vector1.X * vector2.Y;
        }

        public static float AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            double y = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double x = vector1.X * vector2.X + vector1.Y * vector2.Y;
            return (float)(Math.Atan2(y, x) * 57.295779513082323);
        }

        public static Vector2F operator -(Vector2F v)
        {
            return new Vector2F(-v.X, -v.Y);
        }

        public static Vector2F operator -(Vector2F v1, Vector2F v2)
        {
            return new Vector2F(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2F operator +(Vector2F v1, Vector2F v2)
        {
            return new Vector2F(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2F operator *(Vector2F v, float x)
        {
            return new Vector2F(v.X * x, v.Y * x);
        }

        public static Vector2F operator *(float x, Vector2F v)
        {
            return new Vector2F(v.X * x, v.Y * x);
        }

        public static Vector2F operator /(Vector2F v, float x)
        {
            return new Vector2F(v.X / x, v.Y / x);
        }

        public static bool operator ==(Vector2F v1, Vector2F v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector2F v1, Vector2F v2)
        {
            return !v1.Equals(v2);
        }

        public override string ToString()
        {
            return "[Vector2f]" + " X(" + X + ")" + " Y(" + Y + ")";
        }

        public override bool Equals(object obj)
        {
            return (obj is Vector2F) && Equals((Vector2F)obj);
        }

        public bool Equals(Vector2F other)
        {
            return (X == other.X) &&
                   (Y == other.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^
                   Y.GetHashCode();
        }

        public static explicit operator Vector2(Vector2F v)
        {
            return new Vector2((int)v.X, (int)v.Y);
        }

        public static explicit operator Vector2U(Vector2F v)
        {
            return new Vector2U((uint)v.X, (uint)v.Y);
        }

        public float X;

        public float Y;
    }

}
