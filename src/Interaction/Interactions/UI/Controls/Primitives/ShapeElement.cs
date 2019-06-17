using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Drawing2D;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML.Graphics;
using Pandora.SFML.Native;
using System;

namespace Pandora.Interactions.UI.Controls.Primitives
{
    public abstract class ShapeElement : UIElement
    {
        private readonly GetPointCountCallbackType _getpointcountcallback;
        private readonly GetPointCallbackType _getpointcallback;
        private Texture _texture;

        protected ShapeElement() : base(IntPtr.Zero)
        {
            _getpointcountcallback = new GetPointCountCallbackType(InternalGetPointCount);
            _getpointcallback = new GetPointCallbackType(InternalGetPoint);
            Pointer = NativeSFML.sfShape_create(_getpointcountcallback, _getpointcallback, IntPtr.Zero);

            RegisterBindings();
        }

        public ShapeElement(ShapeElement copy) : base(copy)
        {
            _getpointcountcallback = new GetPointCountCallbackType(InternalGetPointCount);
            _getpointcallback = new GetPointCallbackType(InternalGetPoint);
            Pointer = NativeSFML.sfShape_create(_getpointcountcallback, _getpointcallback, IntPtr.Zero);

            RegisterBindings();

            Texture = copy.Texture;
            TextureRect = copy.TextureRect;
            FillColor = copy.FillColor;
            OutlineColor = copy.OutlineColor;
            OutlineThickness = copy.OutlineThickness;
        }

        #region Bindings

        private void RegisterBindings()
        {
            TextureBinding = Bindings.Create("Texture", () => _texture, (value) => { _texture = value; NativeSFML.sfShape_setTexture(Pointer, value.Pointer, false); });
            TextureRectBinding = Bindings.Create("TextureRect", () => NativeSFML.sfShape_getTextureRect(Pointer), (value) => NativeSFML.sfShape_setTextureRect(Pointer, value));
            FillColorBinding = Bindings.Create("FillColor", () => NativeSFML.sfShape_getFillColor(Pointer), (value) => NativeSFML.sfShape_setFillColor(Pointer, value));
            OutlineColorBinding = Bindings.Create("OutlineColor", () => NativeSFML.sfShape_getOutlineColor(Pointer), (value) => NativeSFML.sfShape_setOutlineColor(Pointer, value));
            OutlineThicknessBinding = Bindings.Create("OutlineThickness", () => NativeSFML.sfShape_getOutlineThickness(Pointer), (value) => NativeSFML.sfShape_setOutlineThickness(Pointer, value));
        }


        public BindingProperty<Texture> TextureBinding { get; private set; }

        public BindingProperty<Rectangle> TextureRectBinding { get; private set; }

        public BindingProperty<Color> FillColorBinding { get; private set; }

        public BindingProperty<Color> OutlineColorBinding { get; private set; }

        public BindingProperty<float> OutlineThicknessBinding { get; private set; }

        #endregion

        protected override void ElementDimensionChange()
        {
            NativeSFML.sfShape_update(Pointer);
        }

        public Texture Texture
        {
            get { return _texture; }
            set { TextureBinding.Value = value; }
        }

        public Rectangle TextureRect
        {
            get { return TextureRectBinding.Value; }
            set { TextureRectBinding.Value = value; }
        }

        public Color FillColor
        {
            get { return FillColorBinding.Value; }
            set { FillColorBinding.Value = value; }
        }

        public Color OutlineColor
        {
            get { return OutlineColorBinding.Value; }
            set { OutlineColorBinding.Value = value; }
        }

        public float OutlineThickness
        {
            get { return OutlineThicknessBinding.Value; }
            set { OutlineThicknessBinding.Value = value; }
        }

        protected internal abstract uint GetPointCount();

        protected internal abstract Vector2F GetPoint(uint index);

        public RectangleF GetLocalBounds()
        {
            return NativeSFML.sfShape_getLocalBounds(Pointer);
        }

        public RectangleF GetGlobalBounds()
        {
            // we don't use the native getGlobalBounds function,
            // because we override the object's transform
            return Transform.TransformRect(GetLocalBounds());
        }

        internal override void InternalRenderUpdate(RenderTargetBase target)
        {
            var states = RenderStates.Default;
            states.Transform *= Transform;
            MarshalData marshaledStates = states.Marshal();

            target.DrawShape(Pointer, ref marshaledStates);
        }

        protected override void Destroy(bool disposing)
        {
            NativeSFML.sfShape_destroy(Pointer);
        }

        private uint InternalGetPointCount(IntPtr userData)
        {
            return GetPointCount();
        }

        private Vector2F InternalGetPoint(uint index, IntPtr userData)
        {
            return GetPoint(index);
        }
    }
}
