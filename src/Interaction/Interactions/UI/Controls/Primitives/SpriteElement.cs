using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Drawing2D;
using Pandora.Interactions.UI.Renderer;
using Pandora.SFML.Native;

namespace Pandora.Interactions.UI.Controls.Primitives
{
    public class SpriteElement : UIElement
    {
        private Texture _texture = null;

        public SpriteElement() : base(NativeSFML.sfSprite_create())
        {
            RegisterBindings();
        }

        public SpriteElement(Texture texture) : base(NativeSFML.sfSprite_create())
        {
            RegisterBindings();
            Texture = texture;
        }

        public SpriteElement(Texture texture, Rectangle rectangle) : base(NativeSFML.sfSprite_create())
        {
            RegisterBindings();
            Texture = texture;
            TextureRect = rectangle;
        }

        public SpriteElement(SpriteElement copy) : base(copy)
        {
            Pointer = NativeSFML.sfSprite_copy(copy.Pointer);
            RegisterBindings();
            Texture = copy.Texture;
        }

        #region Bindings

        private void RegisterBindings()
        {
            ColorBinding = Bindings.Create("Color", () => NativeSFML.sfSprite_getColor(Pointer), (value) => NativeSFML.sfSprite_setColor(Pointer, value));
            TextureBinding = Bindings.Create("Texture", () => _texture, (value) => NativeSFML.sfSprite_setTexture(Pointer, value != null ? value.Pointer : IntPtr.Zero, false));
            TextureRectBinding = Bindings.Create("TextureRect", () => NativeSFML.sfSprite_getTextureRect(Pointer), (value) => NativeSFML.sfSprite_setTextureRect(Pointer, value));
        }

        public BindingProperty<Color> ColorBinding { get; private set; }

        public BindingProperty<Texture> TextureBinding { get; private set; }

        public BindingProperty<Rectangle> TextureRectBinding { get; private set; }

        #endregion

        public Color Color
        {
            get { return NativeSFML.sfSprite_getColor(Pointer); }
            set { NativeSFML.sfSprite_setColor(Pointer, value); }
        }

        public Texture Texture
        {
            get { return _texture; }
            set { _texture = value; NativeSFML.sfSprite_setTexture(Pointer, value != null ? value.Pointer : IntPtr.Zero, false); }
        }

        public Rectangle TextureRect
        {
            get { return NativeSFML.sfSprite_getTextureRect(Pointer); }
            set { NativeSFML.sfSprite_setTextureRect(Pointer, value); }
        }

        public RectangleF GetLocalBounds()
        {
            return NativeSFML.sfSprite_getLocalBounds(Pointer);
        }

        public RectangleF GetGlobalBounds()
        {
            // we don't use the native getGlobalBounds function,
            // because we override the object's transform
            return Transform.TransformRect(GetLocalBounds());
        }

        public override string ToString()
        {
            return "[Sprite]" + " Color(" + Color + ")" + " Texture(" + Texture + ")" + " TextureRect(" + TextureRect + ")";
        }

        internal override void InternalRenderUpdate(RenderTargetBase target)
        {
            var states = RenderStates.Default;
            states.Transform *= Transform;
            var marshaledStates = states.Marshal();

            target.DrawSprite(Pointer, ref marshaledStates);
        }

        protected override void Destroy(bool disposing)
        {
            NativeSFML.sfSprite_destroy(Pointer);
        }
    }
}
