using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Controls.Primitives
{
    public class CircleElement : ShapeElement
    {
        private float _radius;
        private uint _pointcount;

        public CircleElement() : this(0)
        { }

        public CircleElement(float radius) : this(radius, 30)
        { }

        public CircleElement(float radius, uint pointCount)
        {
            RegisterBindings();

            Radius = radius;
            SetPointCount(pointCount);
        }

        public CircleElement(CircleElement copy) : base(copy)
        {
            RegisterBindings();

            Radius = copy.Radius;
            SetPointCount(copy.GetPointCount());
        }

        #region Bindings

        private void RegisterBindings()
        {
            RadiusBinding = Bindings.Create("Radius", () => _radius, (value) => _radius = value);
            RadiusBinding.BindingPropertyChanged += delegate (BindingProperty sender, float value) { ElementDimensionChange(); };
        }

        public BindingProperty<float> RadiusBinding { get; private set; }

        #endregion

        public float Radius
        {
            get { return _radius; }
            set { RadiusBinding.Value = value; }
        }

        public void SetPointCount(uint count)
        {
            _pointcount = count;
            ElementDimensionChange();
        }

        protected internal override uint GetPointCount()
        {
            return _pointcount;
        }

        protected internal override Vector2F GetPoint(uint index)
        {
            float angle = (float)(index * 2 * Math.PI / _pointcount - Math.PI / 2);
            float x = (float)Math.Cos(angle) * _radius;
            float y = (float)Math.Sin(angle) * _radius;

            return new Vector2F(_radius + x, _radius + y);
        }
    }
}
