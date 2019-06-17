using Pandora.SFML.Graphics;
using System;

namespace Pandora.Interactions.UI.Drawing
{
    public struct Thickness : IEquatable<Thickness>
    {
        private float _left;
        private float _top;
        private float _right;
        private float _bottom;

        public Thickness(float uniformLength)
        {
            _bottom = uniformLength;
            _right = uniformLength;
            _top = uniformLength;
            _left = uniformLength;
        }

        public Thickness(float left, float top, float right, float bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        public static readonly Thickness NoSet = new Thickness(-1);

        internal bool IsZero { get { return FloatUtil.IsZero(Left) && FloatUtil.IsZero(Top) && FloatUtil.IsZero(Right) && FloatUtil.IsZero(Bottom); } }

        internal bool IsUniform { get { return FloatUtil.AreClose(Left, Top) && FloatUtil.AreClose(Left, Right) && FloatUtil.AreClose(Left, Bottom); } }

        public float Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public float Top
        {
            get { return _top; }
            set { _top = value; }
        }

        public float Right
        {
            get { return _right; }
            set { _right = value; }
        }

        public float Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        internal Vector2F Size { get { return new Vector2F(_left + _right, _top + _bottom); } }

        internal Vector2F Position { get { return new Vector2F(_left, _top); } }

        public override bool Equals(object obj)
        {
            if (obj is Thickness t)
            {
                return this == t;
            }
            return false;
        }

        public bool Equals(Thickness thickness)
        {
            return this == thickness;
        }

        public override int GetHashCode()
        {
            return _left.GetHashCode() ^ _top.GetHashCode() ^ _right.GetHashCode() ^ _bottom.GetHashCode();
        }

        public static bool operator ==(Thickness t1, Thickness t2)
        {
            return (t1._left == t2._left
                || (FloatUtil.IsNaN(t1._left) && FloatUtil.IsNaN(t2._left))) && (t1._top == t2._top
                || (FloatUtil.IsNaN(t1._top) && FloatUtil.IsNaN(t2._top))) && (t1._right == t2._right
                || (FloatUtil.IsNaN(t1._right) && FloatUtil.IsNaN(t2._right))) && (t1._bottom == t2._bottom
                || (FloatUtil.IsNaN(t1._bottom) && FloatUtil.IsNaN(t2._bottom)));
        }

        public static bool operator !=(Thickness t1, Thickness t2)
        {
            return !(t1 == t2);
        }
    }
}
