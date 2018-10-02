using Pandora.Interactions.Bindings;
using Pandora.Interactions.UI.Drawing;
using Pandora.Interactions.UI.Renderer;
using Pandora.Interactions.UI.Styles;
using Pandora.Interactions.UI.Styles.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI
{
    public class Control : ControlElement
    {
        public delegate void ControlEventDelegate(Control control);

        public Control()
        { }

        public new Control Parent { get { return (Control)base.Parent; } }

        protected internal int ControlLevel
        {
            get
            {
                if (Parent is Control control)
                {
                    return Parent.ControlLevel + 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        internal override void InternalOnLoad(SceneHandler handler)
        {
            base.InternalOnLoad(handler);
        }

        protected override void Destroy(bool disposing)
        { }
    }
}
