using Pandora.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.Components
{
    public abstract class ComponentBase
    {
        protected internal virtual void OnSystemUpdate(PandoraRuntimeHost host, float ms, float s)
        {

        }
    }
}
