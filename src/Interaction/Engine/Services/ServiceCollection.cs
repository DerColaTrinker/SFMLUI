﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Engine.Services
{
    public sealed class ServiceCollection
    {
        private HashSet<RuntimeService> _services = new HashSet<RuntimeService>();
        private PandoraRuntimeHost _runtime;

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
            var result = _services.Remove(item);
            if (result)
            {
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
