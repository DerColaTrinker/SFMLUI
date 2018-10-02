using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3F : IEquatable<Vector3F>
    {
        public Vector3F(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3F operator -(Vector3F v)
        {
            return new Vector3F(-v.X, -v.Y, -v.Z);
        }

        public static Vector3F operator -(Vector3F v1, Vector3F v2)
        {
            return new Vector3F(v1.X - v2.X, v1.Y - v2.X, v1.Z - v2.Z);
        }

        public static Vector3F operator +(Vector3F v1, Vector3F v2)
        {
            return new Vector3F(v1.X + v2.X, v1.Y + v2.X, v1.Z + v2.Z);
        }

        public static Vector3F operator *(Vector3F v, float x)
        {
            return new Vector3F(v.X * x, v.Y * x, v.Z * x);
        }

        public static Vector3F operator *(float x, Vector3F v)
        {
            return new Vector3F(v.X * x, v.Y * x, v.Z * x);
        }

        public static Vector3F operator /(Vector3F v, float x)
        {
            return new Vector3F(v.X / x, v.Y / x, v.Z / x);
        }

        public static bool operator ==(Vector3F v1, Vector3F v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector3F v1, Vector3F v2)
        {
            return !v1.Equals(v2);
        }

        public override string ToString()
        {
            return "[Vector3f]" + " X(" + X + ")" + " Y(" + Y + ")" + " Z(" + Z + ")";
        }

        public override bool Equals(object obj)
        {
            return (obj is Vector3F) && Equals((Vector3F)obj);
        }

        public bool Equals(Vector3F other)
        {
            return (X == other.X) &&
                   (Y == other.Y) &&
                   (Z == other.Z);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^
                   Y.GetHashCode() ^
                   Z.GetHashCode();
        }

        public float X;

        public float Y;

        public float Z;
    }
}
