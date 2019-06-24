using System.Collections.Generic;
using System.Linq;

namespace Pandora.Engine.Services
{
    public sealed class ServiceCollection
    {
        private readonly HashSet<RuntimeService> _services = new HashSet<RuntimeService>();
        private readonly PandoraRuntimeHost _runtime;

        public ServiceCollection(PandoraRuntimeHost runtime)
        {
            _runtime = runtime;
        }

        internal ServiceCollection()
        { }

        public int Count => _services.Count;

        public bool IsReadOnly => true;

        public void Add(RuntimeService item)
        {
            lock (_services)
            {
                if (_services.Add(item))
                {
                    Logger.Debug($"[Service] Add service '{item.GetType().Name}'");
                    item.Runtime = _runtime;
                    item.StopRequest += _runtime.InternalStopRequest;
                }
            }
        }
        public void Clear()
        {
            lock (_services)
            {
                foreach (var item in _services)
                {
                    item.Runtime = null;
                    item.StopRequest -= _runtime.InternalStopRequest;
                }

                _services.Clear();

                Logger.Debug($"[Service] Remove all servces");
            }
        }

        public bool Contains(RuntimeService item)
        {
            return _services.Contains(item);
        }

        public void CopyTo(RuntimeService[] array, int arrayIndex)
        {
            _services.CopyTo(array, arrayIndex);
        }

        public bool Remove(RuntimeService item)
        {
            lock (_services)
            { }
            var result = _services.Remove(item);
            if (result)
            {
                Logger.Debug($"[Service] Remove service '{item.GetType().Name}'");
                item.Runtime = null;
                item.StopRequest += _runtime.InternalStopRequest;
            }

            return result;
        }

        public IEnumerable<RuntimeService> Services
        {
            get => _services.ToArray();
        }
    }
}
