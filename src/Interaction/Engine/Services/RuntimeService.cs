using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Engine.Services
{
    public delegate void RuntimeServiceDelegate(RuntimeService service);

    public abstract class RuntimeService : IDisposable
    {
        public event RuntimeServiceDelegate StopRequest;

        protected internal PandoraRuntimeHost Runtime { get; internal set; }

        protected internal abstract void Initialize(out bool success);

        protected internal abstract void Start();

        protected internal abstract void Stop();

        protected internal abstract bool StopRequested();

        public void InitiateStopRequest()
        {
            StopRequest?.Invoke(this);
        }

        protected internal abstract void SystemUpdate(PandoraRuntimeHost host, float ms,float s);

        public virtual void Dispose()
        {
            // Nichts machen, kann vom Service verwenden werden
        }
    }
}
