﻿using Pandora.Interactions.Controller;
using Pandora.Interactions.UI.Animations;
using System;
using System.Linq;

namespace Pandora.Interactions.UI.Design
{
    public class Trigger
    {
        public Trigger()
        {
            Groupname = string.Empty;
        }

        public Animation Animation { get; internal set; }

        public ControlElement Control { get; internal set; }

        public string Groupname { get; internal set; }

        internal virtual void MouseEvents(ControlElement element)
        {
            Start();
        }

        internal virtual void MouseButtonEvents(ControlElement element, MouseButton button, float x, float y)
        {
            Start();
        }

        public void Start()
        {
            // Stop all animations with the groupname
            if (!string.IsNullOrEmpty(Groupname))
                Control.Triggers.Stop(Groupname);
            //AnimationHandler.StopRange(Control.Animations.Where(m => m.Groupname == Groupname));

            AnimationHandler.Start(Animation);
        }

        public void Stop()
        {
            AnimationHandler.Stop(Animation);
        }
    }
}
