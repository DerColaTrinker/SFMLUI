using Pandora.Interactions.Bindings;
using Pandora.Interactions.Caching;
using Pandora.Interactions.UI.Controls;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI
{
    public abstract class Visual : ObjectPointer
    {
        private bool _transformneedupdate = true;
        private bool _inverseNneedupdate = true;

        private Transform _transform;
        private Transform _inversetransform;

        private Vector2F _position;
        private Vector2F _size;
        private Vector2F _screenposition = new Vector2F(0, 0);

        private float _rotation = 0;
        private Vector2F _scale = new Vector2F(1, 1);
        private Vector2F _origin = new Vector2F(0, 0);
        private Vector2F? _positionoffset;

        private Visibilities _visibility = Visibilities.Display;
        private bool _autoscaleonparent = false;

        internal Visual(IntPtr pointer) : base(pointer)
        {
            RegisterBindings();
        }

        internal Visual() : base(IntPtr.Zero)
        {
            RegisterBindings();
        }

        internal Visual(Visual copy) : base(IntPtr.Zero)
        {
            Origin = copy.Origin;
            _screenposition = copy.ScreenPosition;
            Size = copy.Size;
            Rotation = copy.Rotation;
            Scale = copy.Scale;
            InvalidTransformation();

            RegisterBindings();
        }

        #region Bindings

        private void RegisterBindings()
        {
            PositionBinding = new BindingProperty<Vector2F>("Position", () => _position, (value) => _position = value);
            PositionBinding.BindingPropertyChanged += delegate (BindingProperty property, Vector2F value) { UpdatePosition(); };

            SizeBinding = new BindingProperty<Vector2F>("Size", () => _size, (value) => _size = value);

            PositionOffsetBinding = new BindingProperty<Vector2F?>("PositionOffset", () => _positionoffset, (value) => _positionoffset = value);
            PositionOffsetBinding.BindingPropertyChanged += delegate (BindingProperty property, Vector2F? value) { InvalidTransformation(); };

            VisibilityBinding = new BindingProperty<Visibilities>("Visibility", () => _visibility, (value) => _visibility = value);
            VisibilityBinding.BindingPropertyChanged += delegate (BindingProperty property, Visibilities value) { UpdatePosition(); };

            AutoScaleOnParentBinding = new BindingProperty<bool>("AutoScaleOnParent", () => _autoscaleonparent, (value) => _autoscaleonparent = value);
            AutoScaleOnParentBinding.BindingPropertyChanged += delegate (BindingProperty property, bool value) { InvalidTransformation(); };

            RotationBinding = new BindingProperty<float>("Rotation", () => _rotation, (value) => _rotation = value % 360F);
            RotationBinding.BindingPropertyChanged += delegate (BindingProperty property, float value) { InvalidTransformation();};

            ScaleBinding = new BindingProperty<Vector2F>("Scale", () => _scale, (value) => _scale = value);
            ScaleBinding.BindingPropertyChanged += delegate (BindingProperty property, Vector2F value) { InvalidTransformation(); };

            OriginBinding = new BindingProperty<Vector2F>("Origin", () => _origin, (value) => _origin = value);
            OriginBinding.BindingPropertyChanged += delegate (BindingProperty property, Vector2F value) { InvalidTransformation(); };
        }

        public BindingProperty<Vector2F> PositionBinding { get; private set; }

        public BindingProperty<Vector2F> SizeBinding { get; private set; }

        public BindingProperty<Vector2F?> PositionOffsetBinding { get; private set; }

        public BindingProperty<Visibilities> VisibilityBinding { get; private set; }

        public BindingProperty<bool> AutoScaleOnParentBinding { get; private set; }

        public BindingProperty<float> RotationBinding { get; private set; }

        public BindingProperty<Vector2F> ScaleBinding { get; private set; }

        public BindingProperty<Vector2F> OriginBinding { get; private set; }

        #endregion

        internal virtual void InternalSetParent(Visual parent)
        {
            // Scene has no parent
            if (this is Scene) return;
            Parent = parent;
        }

        public Visual Parent { get; private set; }

        public Visibilities Visibility
        {
            get { return VisibilityBinding.Value; }
            set { VisibilityBinding.Value = value; }
        }

        public bool AutoScaleOnParent { get; set; }

        public virtual Vector2F Position
        {
            get { return PositionBinding.Value; }
            set { PositionBinding.Value = value; }
        }

        internal virtual Vector2F ScreenPosition
        {
            get
            {
                return _screenposition;
            }
            //set
            //{
            //    _position = value;
            //    UpdatePositionTransormation();
            //}
        }

        protected Vector2F? PositionOffset
        {
            get { return _positionoffset; }
            set { PositionOffsetBinding.Value = value; }
        }

        public virtual Vector2F Size
        {
            get { return _size; }
            set { SizeBinding.Value = value; }
        }

        internal RectangleF ScreenBound => new RectangleF(ScreenPosition, Size);

        protected internal bool HitTest(Vector2F position)
        {
            return ScreenBound.Contains(position);
        }

        protected internal bool HitTest(float x, float y)
        {
            return ScreenBound.Contains(x, y);
        }

        public virtual float Rotation
        {
            get { return _rotation; }
            set { RotationBinding.Value = value; }
        }

        public virtual Vector2F Scale
        {
            get { return _scale; }
            set { ScaleBinding.Value = value; }
        }

        public virtual Vector2F Origin
        {
            get { return _origin; }
            set { OriginBinding.Value = value; }
        }

        protected internal virtual void UpdatePosition()
        {
            // Update internal screen position
            switch (Visibility)
            {
                case Visibilities.Display:
                case Visibilities.Hidden:
                    if (Parent == null)
                        _screenposition = Position;
                    else
                        _screenposition = Parent.ScreenPosition + Position;
                    break;

                case Visibilities.Collapsed:
                    _screenposition = Position;
                    break;
            }

            // Größe anpassen
            if (AutoScaleOnParent && Parent != null)
                Size = Parent.Size;

            _transformneedupdate = true;
            _inverseNneedupdate = true;
        }

        private void InvalidTransformation()
        {
            _transformneedupdate = true;
            _inverseNneedupdate = true;
        }

        internal virtual Transform Transform
        {
            get
            {
                if (_transformneedupdate)
                {
                    var postion = _positionoffset.HasValue ? _screenposition + _positionoffset.Value : _screenposition;
                    _transformneedupdate = false;

                    float angle = -_rotation * 3.141592654F / 180.0F;
                    float cosine = (float)Math.Cos(angle);
                    float sine = (float)Math.Sin(angle);
                    float sxc = _scale.X * cosine;
                    float syc = _scale.Y * cosine;
                    float sxs = _scale.X * sine;
                    float sys = _scale.Y * sine;
                    float tx = -_origin.X * sxc - _origin.Y * sys + postion.X;
                    float ty = _origin.X * sxs - _origin.Y * syc + postion.Y;

                    _transform = new Transform(sxc, sys, tx, -sxs, syc, ty, 0.0F, 0.0F, 1.0F);
                }

                return _transform;
            }
        }

        internal virtual Transform InverseTransform
        {
            get
            {
                if (_inverseNneedupdate)
                {
                    _inversetransform = Transform.GetInverse();
                    _inverseNneedupdate = false;
                }

                return _inversetransform;
            }
        }
    }
}

