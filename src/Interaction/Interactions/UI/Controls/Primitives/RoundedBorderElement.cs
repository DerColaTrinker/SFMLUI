using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Controls.Primitives
{
    public class RoundedBorderElement : ShapeElement
    {
        private float _radius = 3F;
        private uint _cornerpointcount;

        public RoundedBorderElement() : this(new Vector2F(0, 0))
        { }

        public RoundedBorderElement(Vector2F size)
        {
            RegisterBindings();

            Size = size;
            CornerPointerCount = 5;
        }

        public RoundedBorderElement(RoundedBorderElement copy) : base(copy)
        {
            RegisterBindings();

            Size = copy.Size;
            Radius = copy.Radius;
            CornerPointerCount = copy.CornerPointerCount;
        }

        private void RegisterBindings()
        {
            RadiusBinding = new BindingProperty<float>("Radius", () => _radius, (value) => _radius = value);
            RadiusBinding.BindingPropertyChanged += delegate (BindingProperty sender, float value) { ElementDimensionChange(); };

            CornerPointerCountBinding = new BindingProperty<uint>("CornerPointerCount", () => _cornerpointcount, (value) => _cornerpointcount = value);
            CornerPointerCountBinding.BindingPropertyChanged += delegate (BindingProperty sender, uint value) { ElementDimensionChange(); };

            SizeBinding.BindingPropertyChanged += delegate (BindingProperty sender, Vector2F value) { ElementDimensionChange(); };
        }

        public BindingProperty<float> RadiusBinding { get; private set; }

        public BindingProperty<uint> CornerPointerCountBinding { get; private set; }

        public float Radius { get { return _radius; } set { RadiusBinding.Value = value; } }

        public uint CornerPointerCount { get { return _cornerpointcount; } set { CornerPointerCountBinding.Value = value; } }

        protected internal override uint GetPointCount()
        {
            return _cornerpointcount * 4;
        }

        protected internal override Vector2F GetPoint(uint index)
        {
            var cornerpointcount = _cornerpointcount * 4;

            if (index >= cornerpointcount) return new Vector2F();

            var deltaangle = 90F / (cornerpointcount - 1);
            var center = new Vector2F();
            var centerindex = index / cornerpointcount;

            switch (centerindex)
            {
                case 0: center.X = base.Size.X - _radius; center.Y = _radius; break;
                case 1: center.X = _radius; center.Y = _radius; break;
                case 2: center.X = _radius; center.Y = base.Size.Y - _radius; break;
                case 3: center.X = base.Size.X - _radius; center.Y = base.Size.Y - _radius; break;
            }

            return new Vector2F(_radius * (float)Math.Cos(deltaangle * (index - centerindex) * (float)Math.PI / 180) + center.X, -_radius * (float)Math.Sin(deltaangle * (index - centerindex) * (float)Math.PI / 180) + center.Y);
        }
    }
}