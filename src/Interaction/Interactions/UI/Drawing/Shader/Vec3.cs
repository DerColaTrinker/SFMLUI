using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing.Shader
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vec3
    {
        public static implicit operator Vec3(Vector3F vec)
        {
            return new Vec3(vec);
        }

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3(Vector3F vec)
        {
            X = vec.X;
            Y = vec.Y;
            Z = vec.Z;
        }

        public float X;

        public float Y;

        public float Z;
    }
}
