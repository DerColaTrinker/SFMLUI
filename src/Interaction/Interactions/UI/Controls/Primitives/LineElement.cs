using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;

namespace Pandora.Interactions.UI.Controls.Primitives
{
    public class LineElement : ShapeElement
    {
        public LineElement() : this(new Vector2F(0, 0))
        { }

        public LineElement(Vector2F size)
        {
            RegisterBindings();

            Size = size;
        }

        public LineElement(LineElement copy) : base(copy)
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
                case 1: return new Vector2F(base.Size.X, base.Size.Y);
            }
        }

        protected internal override uint GetPointCount()
        {
            return 2;
        }
    }
}
