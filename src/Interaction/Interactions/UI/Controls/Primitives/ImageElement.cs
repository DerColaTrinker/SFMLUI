using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
