using Pandora.Interactions.UI.Drawing2D;

namespace Pandora.Interactions.UI.Controls.Primitives
{
    public class ImageElement : RectElement
    {
        public ImageElement() : base()
        { }

        public ImageElement(Texture texture) : base(texture.Size)
        {
            Texture = texture;
        }
    }
}
