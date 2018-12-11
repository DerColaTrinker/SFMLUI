using Pandora.SFML.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Transform : IEquatable<Transform>
    {
        public Transform(float a00, float a01, float a02, float a10, float a11, float a12, float a20, float a21, float a22)
        {
            m00 = a00;
            m01 = a01;
            m02 = a02;
            m10 = a10;
            m11 = a11;
            m12 = a12;
            m20 = a20;
            m21 = a21;
            m22 = a22;
        }

        public Transform GetInverse()
        {
            return NativeSFML.sfTransform_getInverse(ref this);
        }

        public Vector2F TransformPoint(float x, float y)
        {
            return TransformPoint(new Vector2F(x, y));
        }

        public Vector2F TransformPoint(Vector2F point)
        {
            return NativeSFML.sfTransform_transformPoint(ref this, point);
        }

        public RectangleF TransformRect(RectangleF rectangle)
        {
            return NativeSFML.sfTransform_transformRect(ref this, rectangle);
        }

        public void Combine(Transform transform)
        {
            NativeSFML.sfTransform_combine(ref this, ref transform);
        }

        public void Translate(float x, float y)
        {
            NativeSFML.sfTransform_translate(ref this, x, y);
        }

        public void Translate(Vector2F offset)
        {
            Translate(offset.X, offset.Y);
        }

        public void Rotate(float angle)
        {
            NativeSFML.sfTransform_rotate(ref this, angle);
        }

        public void Rotate(float angle, float centerX, float centerY)
        {
            NativeSFML.sfTransform_rotateWithCenter(ref this, angle, centerX, centerY);
        }

        public void Rotate(float angle, Vector2F center)
        {
            Rotate(angle, center.X, center.Y);
        }

        public void Scale(float scaleX, float scaleY)
        {
            NativeSFML.sfTransform_scale(ref this, scaleX, scaleY);
        }

        public void Scale(float scaleX, float scaleY, float centerX, float centerY)
        {
            NativeSFML.sfTransform_scaleWithCenter(ref this, scaleX, scaleY, centerX, centerY);
        }

        public void Scale(Vector2F factors)
        {
            Scale(factors.X, factors.Y);
        }

        public void Scale(Vector2F factors, Vector2F center)
        {
            Scale(factors.X, factors.Y, center.X, center.Y);
        }

        public static Transform operator *(Transform left, Transform right)
        {
            left.Combine(right);
            return left;
        }

        public static Vector2F operator *(Transform left, Vector2F right)
        {
            return left.TransformPoint(right);
        }

        public static Transform Identity
        {
            get
            {
                return new Transform(1, 0, 0, 0, 1, 0, 0, 0, 1);
            }
        }

        public override bool Equals(object obj) => (obj is Transform) && Equals((Transform)obj);

        public bool Equals(Transform transform)
        {
            return NativeSFML.sfTransform_equal(ref this, ref transform);
        }

        public override string ToString()
        {
            return string.Format("[Transform]" + " Matrix(" + "{0}, {1}, {2}," + "{3}, {4}, {5}," + "{6}, {7}, {8}, )", m00, m01, m02, m10, m11, m12, m20, m21, m22);
        }

        internal float m00;
        internal float m01;
        internal float m02;
        internal float m10;
        internal float m11;
        internal float m12;
        internal float m20;
        internal float m21;
        internal float m22;
    }
}
