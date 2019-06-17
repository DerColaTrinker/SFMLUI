using Pandora.Interactions.UI.Drawing2D;
using System.IO;

namespace Pandora.Interactions.Caching
{
    public sealed class FontCache : CacheCollection<Font>
    {
        public void Load(string key, string filename)
        {
            var font = new Font(filename);
            Add(key, font);
        }

        public void Load(string key, Stream stream)
        {
            var font = new Font(stream);
            Add(key, font);
        }

        public Font TryGetOrLoad(string key, string filename)
        {
            var font = (Font)null;

            if (TryGetValue(key, out font))
                return font;

            font = new Font(filename);
            Add(key, font);

            return font;
        }

        public Font TryGetOrLoad(string key, Stream stream)
        {
            var font = (Font)null;

            if (TryGetValue(key, out font))
                return font;

            font = new Font(stream);
            Add(key, font);

            return font;
        }
    }
}
