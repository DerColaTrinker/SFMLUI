using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing.Shader
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IVec2
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Implicit cast from <see cref="Vector2i"/> to <see cref="IVec2"/>
        /// </summary>
        public static implicit operator IVec2(Vector2 vec)
        {
            return new IVec2(vec);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct the <see cref="IVec2"/> from its coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        ////////////////////////////////////////////////////////////
        public IVec2(int x, int y)
        {
            X = x;
            Y = y;
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct the <see cref="IVec2"/> from a standard SFML <see cref="Vector2i"/>
        /// </summary>
        /// <param name="vec">A standard SFML 2D integer vector</param>
        ////////////////////////////////////////////////////////////
        public IVec2(Vector2 vec)
        {
            X = vec.X;
            Y = vec.Y;
        }

        /// <summary>Horizontal component of the vector</summary>
        public int X;

        /// <summary>Vertical component of the vector</summary>
        public int Y;
    }

}
