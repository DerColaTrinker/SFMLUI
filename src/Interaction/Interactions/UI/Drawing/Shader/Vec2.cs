using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing.Shader
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vec2
    {
        public static implicit operator Vec2(Vector2F vec)
        {
            return new Vec2(vec);
        }

        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vec2(Vector2F vec)
        {
            X = vec.X;
            Y = vec.Y;
        }

        public float X;

        public float Y;
    }
}
