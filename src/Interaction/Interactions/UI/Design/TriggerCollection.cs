using Pandora.Interactions.Controller;
using Pandora.Interactions.UI.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Design
{
    public sealed class TriggerCollection
    {
        private readonly HashSet<Trigger> _triggers = new HashSet<Trigger>();

        public TriggerCollection(ControlElement controlElement)
        { }

        internal void StopOtherAnimationsByGroupname(string groupname ,Animation animation)
        {
            var triggers = _triggers.Where(m => m.Groupname == groupname && m.Animation != animation);

            foreach (var trigger in triggers)
            {
                trigger.Stop();
            }
        }

        public void Add(Trigger trigger)
        {
            _triggers.Add(trigger);
        }

        public void CreateTrigger(ControlElement control, string groupname, TriggerEvents triggerevent, Animation animation)
        {
            Add(DesignHandler.CreateTrigger(control, groupname, triggerevent, animation));
        }

        public void CreateMouseButtonTrigger(ControlElement control, string groupname, TriggerEvents triggerevent, MouseButton button, Animation animation)
        {
            Add(DesignHandler.CreateMouseButtonTrigger(control, groupname, triggerevent, button, animation));
        }
    }
}
