using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;

namespace Pandora.Interactions.UI
{
    public abstract class ControlContainer : Control, IContainer
    {
        protected ControlContainer()
        {
            Controls = new ControlCollection(this);
        }

        public ControlCollection Controls { get; }    
    }
}

