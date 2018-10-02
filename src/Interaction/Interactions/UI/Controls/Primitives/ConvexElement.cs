using Pandora.Interactions.UI.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Controls.Primitives
{
    public class ConvexShape : ShapeElement
    {
        private Vector2F[] _points;

        public ConvexShape() : this(0)
        { }

        public ConvexShape(uint pointCount)
        {
            SetPointCount(pointCount);
        }

        public ConvexShape(ConvexShape copy) : base(copy)
        {
            SetPointCount(copy.GetPointCount());
            for (uint i = 0; i < copy.GetPointCount(); ++i)
                SetPoint(i, copy.GetPoint(i));
        }

        protected internal override uint GetPointCount()
        {
            return (uint)_points.Length;
        }

        public void SetPointCount(uint count)
        {
            Array.Resize(ref _points, (int)count);
            ElementDimensionChange();
        }

        protected internal override Vector2F GetPoint(uint index)
        {
            return _points[index];
        }

        public void SetPoint(uint index, Vector2F point)
        {
            _points[index] = point;
            ElementDimensionChange();
        }
    }
}
