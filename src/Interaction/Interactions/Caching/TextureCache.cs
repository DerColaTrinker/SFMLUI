using Pandora.Interactions.UI.Drawing2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.Caching
{
    public sealed class TextureCache : CacheCollection<Texture>
    {
        public Texture LoadTexture(string path)
        {
            return new Texture(path);
        }
    }
}
