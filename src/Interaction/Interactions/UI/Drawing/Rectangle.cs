using System;
using System.Runtime.InteropServices;

namespace Pandora.Interactions.UI.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle : IEquatable<Rectangle>
    {
        public Rectangle(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public Rectangle(Vector2 position, Vector2 size)
            : this(position.X, position.Y, size.X, size.Y)
        {
        }

        public bool Contains(int x, int y)
        {
            int minX = Math.Min(Left, Left + Width);
            int maxX = Math.Max(Left, Left + Width);
            int minY = Math.Min(Top, Top + Height);
            int maxY = Math.Max(Top, Top + Height);

            return (x >= minX) && (x < maxX) && (y >= minY) && (y < maxY);
        }

        public bool Intersects(Rectangle rect)
        {
            Rectangle overlap;
            return Intersects(rect, out overlap);
        }

        public bool Intersects(Rectangle rect, out Rectangle overlap)
        {
            // Rectangles with negative dimensions are allowed, so we must handle them correctly

            // Compute the min and max of the first rectangle on both axes
            int r1MinX = Math.Min(Left, Left + Width);
            int r1MaxX = Math.Max(Left, Left + Width);
            int r1MinY = Math.Min(Top, Top + Height);
            int r1MaxY = Math.Max(Top, Top + Height);

            // Compute the min and max of the second rectangle on both axes
            int r2MinX = Math.Min(rect.Left, rect.Left + rect.Width);
            int r2MaxX = Math.Max(rect.Left, rect.Left + rect.Width);
            int r2MinY = Math.Min(rect.Top, rect.Top + rect.Height);
            int r2MaxY = Math.Max(rect.Top, rect.Top + rect.Height);

            // Compute the intersection boundaries
            int interLeft = Math.Max(r1MinX, r2MinX);
            int interTop = Math.Max(r1MinY, r2MinY);
            int interRight = Math.Min(r1MaxX, r2MaxX);
            int interBottom = Math.Min(r1MaxY, r2MaxY);

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
            return "[IntRect]" + " Left(" + Left + ")" + " Top(" + Top + ")" + " Width(" + Width + ")" + " Height(" + Height + ")";
        }

        public override bool Equals(object obj)
        {
            return (obj is Rectangle) && Equals((Rectangle)obj);
        }

        public bool Equals(Rectangle other)
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

        public static bool operator ==(Rectangle r1, Rectangle r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(Rectangle r1, Rectangle r2)
        {
            return !r1.Equals(r2);
        }

        public static explicit operator RectangleF(Rectangle r)
        {
            return new RectangleF((float)r.Left, (float)r.Top, (float)r.Width, (float)r.Height);
        }

        public int Left;

        public int Top;

        public int Width;

        public int Height;
    }
}
