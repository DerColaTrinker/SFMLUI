using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RectangleF : IEquatable<RectangleF>
    {
        public RectangleF(float left, float top, float width, float height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public RectangleF(Vector2F position, Vector2F size)
            : this(position.X, position.Y, size.X, size.Y)
        {
        }

        public bool Contains(float x, float y)
        {
            float minX = Math.Min(Left, Left + Width);
            float maxX = Math.Max(Left, Left + Width);
            float minY = Math.Min(Top, Top + Height);
            float maxY = Math.Max(Top, Top + Height);

            return (x >= minX) && (x < maxX) && (y >= minY) && (y < maxY);
        }

        internal bool Contains(Vector2F position)
        {
            return Contains(position.X, position.Y);
        }

        public bool Intersects(RectangleF rect)
        {
            RectangleF overlap;
            return Intersects(rect, out overlap);
        }

        public bool Intersects(RectangleF rect, out RectangleF overlap)
        {
            // Rectangles with negative dimensions are allowed, so we must handle them correctly

            // Compute the min and max of the first rectangle on both axes
            float r1MinX = Math.Min(Left, Left + Width);
            float r1MaxX = Math.Max(Left, Left + Width);
            float r1MinY = Math.Min(Top, Top + Height);
            float r1MaxY = Math.Max(Top, Top + Height);

            // Compute the min and max of the second rectangle on both axes
            float r2MinX = Math.Min(rect.Left, rect.Left + rect.Width);
            float r2MaxX = Math.Max(rect.Left, rect.Left + rect.Width);
            float r2MinY = Math.Min(rect.Top, rect.Top + rect.Height);
            float r2MaxY = Math.Max(rect.Top, rect.Top + rect.Height);

            // Compute the intersection boundaries
            float interLeft = Math.Max(r1MinX, r2MinX);
            float interTop = Math.Max(r1MinY, r2MinY);
            float interRight = Math.Min(r1MaxX, r2MaxX);
            float interBottom = Math.Min(r1MaxY, r2MaxY);

            // If the intersection is valid (positive non zero area), then there is an intersection
            if ((interLeft < interRight) && (interTop < interBottom))
            {
                overlap.Left = interLeft;
                overlap.Top = interTop;
                overlap.Width = interRight - interLeft;
                overlap.Height = interBottom - interTop;
                return true;
            }
            else
            {
                overlap.Left = 0;
                overlap.Top = 0;
                overlap.Width = 0;
                overlap.Height = 0;
                return false;
            }
        }

        public override string ToString()
        {
            return "[FloatRect]" + " Left(" + Left + ")" + " Top(" + Top + ")" + " Width(" + Width + ")" + " Height(" + Height + ")";
        }

        public override bool Equals(object obj)
        {
            return (obj is RectangleF) && Equals((RectangleF)obj);
        }

        public bool Equals(RectangleF other)
        {
            return (Left == other.Left) &&
                   (Top == other.Top) &&
                   (Width == other.Width) &&
                   (Height == other.Height);
        }

        public override int GetHashCode()
        {
            return unchecked((int)((uint)Left ^
                   (((uint)Top << 13) | ((uint)Top >> 19)) ^
                   (((uint)Width << 26) | ((uint)Width >> 6)) ^
                   (((uint)Height << 7) | ((uint)Height >> 25))));
        }

        public static bool operator ==(RectangleF r1, RectangleF r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(RectangleF r1, RectangleF r2)
        {
            return !r1.Equals(r2);
        }

        public static explicit operator Rectangle(RectangleF r)
        {
            return new Rectangle((int)r.Left,
                               (int)r.Top,
                               (int)r.Width,
                               (int)r.Height);
        }

        public float Left;

        public float Top;

        public float Width;

        public float Height;
    }
}