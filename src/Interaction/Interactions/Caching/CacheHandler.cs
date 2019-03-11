using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.Caching
{
    public sealed class CacheHandler
    {
        public CacheHandler()
        {
            Fonts = new FontCache();
            Colors = new ColorCache();
            Textures = new TextureCache();
        }

        public FontCache Fonts { get; }

        public ColorCache Colors { get; }

        public TextureCache Textures { get; }
    }
}
