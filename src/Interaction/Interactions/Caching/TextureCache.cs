using Pandora.Interactions.UI.Drawing2D;

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
