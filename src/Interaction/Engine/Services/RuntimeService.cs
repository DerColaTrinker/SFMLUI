using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Engine.Services
{
    public abstract class RuntimeService
    {
        protected internal abstract void Initialize(out bool success);

        protected internal abstract void Start();

        protected internal abstract void Stop();

        protected internal abstract bool StopRequest();

        protected internal abstract void SystemUpdate(PandoraRuntimeHost pandoraRuntimeHost, RuntimeFrameEventArgs args);
    }
}
