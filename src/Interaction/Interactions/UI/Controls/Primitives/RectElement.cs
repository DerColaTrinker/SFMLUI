using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;

namespace Pandora.Interactions.UI.Controls.Primitives
{
    public class RectElement : ShapeElement
    {
        public RectElement() : this(new Vector2F(0, 0))
        { }

        public RectElement(Vector2F size)
        {
            RegisterBindings();

            Size = size;
        }

        public RectElement(RectElement copy) : base(copy)
        {
            RegisterBindings();

            Size = copy.Size;
        }

        private void RegisterBindings()
        {
            SizeBinding.BindingPropertyChanged += delegate (BindingProperty sender, Vector2F value) { ElementDimensionChange(); };
        }

        protected internal override Vector2F GetPoint(uint index)
        {
            switch (index)
            {
                default:
                case 0: return new Vector2F(0, 0);
                case 1: return new Vector2F(base.Size.X, 0);
                case 2: return new Vector2F(base.Size.X, base.Size.Y);
                case 3: return new Vector2F(0, base.Size.Y);
            }
        }

        protected internal override uint GetPointCount()
        {
            return 4;
        }
    }
}
