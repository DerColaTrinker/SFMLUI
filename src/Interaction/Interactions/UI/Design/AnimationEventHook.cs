﻿using Pandora.Interactions.UI.Animations;
using System.Linq;

namespace Pandora.Interactions.UI.Design
{
    public sealed class AnimationEventHook
    {
        public AnimationEventHook()
        {
            Groupname = string.Empty;
        }

        public Animation Animation { get; internal set; }

        public ControlElement Control { get; internal set; }

        public string Groupname { get; internal set; }

        internal void MouseEvents1(ControlElement element)
        {
            Start();
        }

        public void Start()
        {
            // Stop all animations with the groupname
            if (!string.IsNullOrEmpty(Groupname))
                AnimationHandler.StopRange(Control.Animations.Where(m => m.Groupname == Groupname));

            AnimationHandler.Start(Animation);
        }

        public void Stop()
        {
            AnimationHandler.Stop(Animation);
        }
    }
}
