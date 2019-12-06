using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandora.Interactions.Controller;

namespace Pandora.Interactions.UI.Design
{
    public sealed class MouseButtonTrigger : Trigger
    {
        public MouseButton MouseButton { get; set; }

        internal override void MouseButtonEvents(ControlElement element, MouseButton button, float x, float y)
        {
            if (button == MouseButton)
                base.MouseButtonEvents(element, button, x, y);
        }
    }
}
