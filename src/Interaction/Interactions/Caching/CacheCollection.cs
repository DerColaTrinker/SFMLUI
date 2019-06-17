using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Interactions.Caching
{
    public abstract class CacheCollection<T> : IEnumerable<KeyValuePair<string, T>>
    {
        private Dictionary<string, T> _cache = new Dictionary<string, T>(StringComparer.InvariantCultureIgnoreCase);

        protected CacheCollection()
        {

        }

        public virtual T this[string key] { get => _cache[key]; set => _cache[key] = value; }

        public virtual ICollection<string> Keys => _cache.Keys;

        public virtual ICollection<T> Values => _cache.Values;

        public virtual int Count => _cache.Count;

        public virtual void Add(string key, T value)
        {
            _cache[key] = value;
        }

        public virtual void Clear()
        {
            _cache.Clear();
        }

        public virtual bool Contains(KeyValuePair<string, T> item)
        {
            return _cache.Contains(item);
        }

        public virtual bool ContainsKey(string key)
        {
            return _cache.ContainsKey(key);
        }

        public virtual IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _cache.GetEnumerator();
        }

        public virtual bool Remove(string key)
        {
            return _cache.Remove(key);
        }

        public virtual bool TryGetValue(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
