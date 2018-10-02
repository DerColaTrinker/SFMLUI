﻿using Pandora.Interactions.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Interactions.UI.Animations
{
    public abstract class PropertyAnimation : Animation
    {
        protected PropertyAnimation(BindingProperty property, float time) : base(time)
        {
            Property = property;
        }

        public BindingProperty Property { get; }
    }

    public abstract class PropertyAnimation<T> : PropertyAnimation
    {
        protected PropertyAnimation(BindingProperty<T> property, T targetvalue, float time) : base(property, time)
        {
            TargetValue = targetvalue;
        }

        public new BindingProperty<T> Property { get { return (BindingProperty<T>)base.Property; } }

        public T TargetValue { get; internal set; }

        protected override void OnFinish()
        {
            // Sicherstellen das am Ende der Animation, der Wert auch genau auf das gesetzt wird, wo es hin soll
            Property.Value = TargetValue;
        }
    }
}
