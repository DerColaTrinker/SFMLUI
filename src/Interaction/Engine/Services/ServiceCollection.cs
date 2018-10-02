using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Engine.Services
{
    public sealed class ServiceCollection : ICollection<RuntimeService>
    {
        private HashSet<RuntimeService> _services = new HashSet<RuntimeService>();

        internal ServiceCollection()
        { }

        public int Count => _services.Count;

        public bool IsReadOnly => true;

        public void Add(RuntimeService item)
        {
            _services.Add(item);
        }

        public void Clear()
        {
            _services.Clear();
        }

        public bool Contains(RuntimeService item)
        {
            return _services.Contains(item);
        }

        public void CopyTo(RuntimeService[] array, int arrayIndex)
        {
            _services.CopyTo(array, arrayIndex);
        }

        public IEnumerator<RuntimeService> GetEnumerator()
        {
            return _services.GetEnumerator();
        }

        public bool Remove(RuntimeService item)
        {
            return _services.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _services.GetEnumerator();
        }
    }
}
